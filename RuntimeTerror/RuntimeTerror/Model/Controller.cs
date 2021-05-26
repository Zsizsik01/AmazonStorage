using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Controller
    {

        #region Constructor
        public Controller()
        {
            Init();
        }
        #endregion


        #region Properties

        private Storage storage;

        public Storage Storage
        {
            get { return storage; }
            set { storage = value; }
        }

        private Storage storageReservation;
        public Storage StorageReservation
        {
            get { return storageReservation; }
            set { storageReservation = value; }
        }

        private int item;
        public int Item
        {
            get { return item; }
            set { item = value; }
        }

        private ResultDiary result;

        public ResultDiary Result
        {
            get { return result; }
            set { result = value; }
        }
        private List<Robot> robots;

        public List<Robot> Robots
        {
            get { return robots; }
            set { robots = value; }
        }

        private List<Shelf> itemedShelfs;

        public List<Shelf> ItemedShelfs
        {
            get { return itemedShelfs; }
            set { itemedShelfs = value; }
        }

        private List<Destination> destinations;

        public List<Destination> Destinations
        {
            get { return destinations; }
            set { destinations = value; }
        }

        private List<Dock> docks;

        public List<Dock> Docks
        {
            get { return docks; }
            set { docks = value; }
        }

        private int maxEnergy;
        public int MaxEnergy
        {
            get { return maxEnergy; }
            set { maxEnergy = value; }
        }

        private int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        #endregion



        #region Functions
        /// <summary>
        /// Inicializálja a szimuláció adatait
        /// </summary>
        public void Init()
        {
            storage = new Storage();
            item = 0;
            destinations = new List<Destination>();
            docks = new List<Dock>();
            itemedShelfs = new List<Shelf>();
            robots = new List<Robot>();
            storageReservation = new Storage();
            result = new ResultDiary();
           
        }

        /// <summary>
        /// Ami a Storage mátrixában robot, azt a foglaltsági mátrixban reserved-re állítja.
        /// </summary>
        public void MakeRobotFieldsReserved()
        {
            for (int i = 0; i < storage.SizeX; i++)
            {
                for (int j = 0; j < storage.SizeY; j++)
                {
                    if (storage.Matrix[i, j] == Field.ROBOT)
                    {
                        storageReservation.Matrix[i, j] = Field.RESERVED;
                    }
                }
            }
        }
        /// <summary>
        /// Mikor új elemet adunk a pályához a szerkesztőben, azt hozzáadjuk a megfelelő listánkhoz is.
        /// </summary>
        /// <param name="x">x koordináta</param>
        /// <param name="y">y koordináta</param>
        /// <param name="value">Mező típusa</param>
        public void AddElementToList(int x, int y, Field value)
        {
            switch (value)
            {
                case Field.DESTINATION:
                    Destinations.Add(new Destination(x, y, Item));
                    Item++;
                    break;
                case Field.DOCK:
                    Docks.Add(new Dock(x, y));
                    break;
                case Field.ROBOT:
                    Robots.Add(new Robot(x, y, Direction.LEFT, maxEnergy));
                    result.Energy.Add(0);
                    break;
            }
        }

        /// <summary>
        /// Minden lépésnél ez a függvény fut le, minden robotokra lebontva további függvényeket hív meg, a robot aktuális állapotától függően
        /// </summary>
        public void NextCommand()
        {

            result.Steps++;
            foreach (Robot robot in robots) 
            {
                if (robot.Energy == 0) { throw new Exception(); }
                
                if( robot.EndX == robot.PositionX && robot.EndY == robot.PositionY) 
                {
                    switch(storage.Matrix[robot.PositionX, robot.PositionY])
                    {
                        case Field.ROBOTWITHDESTINATION :
                            DestinationAction(robot);
                        break;
                        case Field.ROBOTWITHSHELFANDDESTINATION:
                            DestinationAction(robot);
                        break;
                        case Field.ROBOTWITHDOCK:
                            DockAction(robot);
                            break;
                        case Field.ROBOTWITHSHELF:
                            ShelfAction(robot);
                            break;
                        case Field.ROBOT:
                            NewTask(robot);
                            break;
                    }
                }
                else 
                {
                    MoveAction(robot);
                }               
            }
        }
        
        /// <summary>
        /// Teszteli hogy ha a robot abba az irányba lép amerre néz, megfelelő mezőre lép-e? Vagy forgatni kell?
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>

        bool TestMove(Robot robot)
        {
            Robot testRobot = new Robot(robot.PositionX,robot.PositionY,robot.Direction,robot.Energy);
            switch (testRobot.Direction)
            {
                case Direction.DOWN:
                    testRobot.Move(testRobot.PositionX +1, testRobot.PositionY );
                    break;
                case Direction.UP:
                    testRobot.Move(testRobot.PositionX - 1, testRobot.PositionY );
                    break;
                case Direction.LEFT:
                    testRobot.Move(testRobot.PositionX , testRobot.PositionY - 1);
                    break;
                case Direction.RIGHT:
                    testRobot.Move(testRobot.PositionX , testRobot.PositionY + 1);
                    break;
            }
            if(testRobot.PositionX == robot.ReservedFields[robot.ReservedFields.Count-1].Item1 &&
               testRobot.PositionY == robot.ReservedFields[robot.ReservedFields.Count-1].Item2)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// TestMovetól függően lépteti vagy forgatja a robotot, lépés esetén felszabadítja a lefoglalt mezőt amin a robot az előzőz lépésben állt.
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>

        void MoveAction(Robot robot)
        {
            result.Energy[robots.IndexOf(robot)]++;
            if (TestMove(robot))
            {
                int lastIndex = robot.ReservedFields.Count-1;
                storageReservation.Matrix[robot.PositionX, robot.PositionY] = Field.EMPTY;
                switch (storage.Matrix[robot.PositionX, robot.PositionY])
                {
                    case Field.ROBOTWITHDESTINATION:
                        storage.Matrix[robot.PositionX, robot.PositionY] = Field.DESTINATION;
                        break;
                    case Field.ROBOTWITHDOCK:
                        storage.Matrix[robot.PositionX, robot.PositionY] = Field.DOCK;
                        break;
                    case Field.ROBOTWITHSHELF:
                        if(robot.ShelfOnIt != null)
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.EMPTY;
                        }
                        else
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.SHELF;
                        }
                        break;
                    case Field.ROBOT:
                        storage.Matrix[ robot.PositionX,robot.PositionY] = Field.EMPTY;
                        break;
                    case Field.ROBOTWITHSHELFANDDESTINATION:
                        storage.Matrix[robot.PositionX, robot.PositionY] = Field.DESTINATION;
                        break;
                }

                robot.Move(robot.ReservedFields[lastIndex].Item1, robot.ReservedFields[lastIndex].Item2); //a robotot a lefoglalt mezőinek következő pontjára mozgatja
  
                robot.ReservedFields.RemoveAt(lastIndex);

                switch (storage.Matrix[robot.PositionX, robot.PositionY])
                {
                    case Field.DESTINATION:
                        if(robot.ShelfOnIt != null)
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOTWITHSHELFANDDESTINATION;
                        }
                        else
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOTWITHDESTINATION;
                        }
                        break;
                    case Field.DOCK:
                        storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOTWITHDOCK;
                        break;
                    case Field.SHELF:
                        storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOTWITHSHELF;
                        break;
                    case Field.EMPTY:
                        if (robot.ShelfOnIt != null)
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOTWITHSHELF;
                        }
                        else
                        {
                            storage.Matrix[robot.PositionX, robot.PositionY] = Field.ROBOT;
                        }
                        
                        break;
                }
            }
            else
            {
                switch (robot.Direction)
                {
                    case Direction.DOWN:
                        Rotate( robot, robot.PositionY, 
                                robot.ReservedFields[robot.ReservedFields.Count-1].Item2,
                                Direction.LEFT,
                                Direction.RIGHT);
                        break;
                    case Direction.UP:
                        Rotate(robot, robot.PositionY,
                                robot.ReservedFields[robot.ReservedFields.Count-1].Item2,
                                Direction.LEFT,
                                Direction.RIGHT);
                        break;
                    case Direction.LEFT:
                        Rotate(robot, robot.PositionX,
                                robot.ReservedFields[robot.ReservedFields.Count-1].Item1,
                                Direction.UP,
                                Direction.DOWN);
                        break;
                    case Direction.RIGHT:
                        Rotate(robot, robot.PositionX,
                                robot.ReservedFields[robot.ReservedFields.Count-1].Item1,
                                Direction.UP,
                                Direction.DOWN);
                        break;
                }
            }
        }
        /// <summary>
        /// A robot töltöttségétől függően tölti tovább a robotot, vagy új útra indítja
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>
        /// <param name="actualPos">Beállítja a robot aktuális pozícióját</param>
        /// <param name="nextPos">Beállítja a robot kívánt pozícióját</param>
        /// <param name="ifTheActPosBigger">Beállítja azt hogy milyen irányba forogjunk, ha nagyobb az aktuális pozíció</param>
        /// <param name="ifTheActPosSmaller">Beállítja azt hogy milyen irányba forogjunk, ha kissebb az aktuális pozíció</param>
        void Rotate(Robot robot, int actualPos , int nextPos, Direction ifTheActPosBigger, Direction ifTheActPosSmaller)
        {
            if(actualPos > nextPos)
            {
                robot.TurnAway(ifTheActPosBigger);
            }
            else
            {
                robot.TurnAway(ifTheActPosSmaller);
            }
        }
        /// <summary>
        /// A robot töltöttségétől függően tölti tovább a robotot, vagy új útra indítja
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>


        void DockAction(Robot robot)
        {
            
            if(robot.Energy == maxEnergy)
            {
                BFS bfs = new BFS(storage,destinations, itemedShelfs,StorageReservation, robot, true);
                bfs.setField(Field.SHELF);
                List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                SetCellsReserved(robot, reservedList);
            }
            else
            {
                if(robot.Energy + maxEnergy/5 > maxEnergy)
                {
                    robot.Charge(maxEnergy - robot.Energy);
                }
                else
                {
                    robot.Charge(maxEnergy / 5);
                }
                
            }
        }

        /// <summary>
        /// Ha megfelelő a robot töltöttsége, új polcért küldi egyébként dokkolni
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>

        void NewTask(Robot robot)
        {
            if (ChargeCheck(robot)) 
            {
                BFS bfs = new BFS(storage, destinations, itemedShelfs, StorageReservation, robot, true);
                bfs.setField(Field.SHELF);
                List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                SetCellsReserved(robot, reservedList);

            }
            else
            {
                BFS bfs = new BFS(storage, destinations, itemedShelfs, StorageReservation, robot, true);
                bfs.setField(Field.DOCK);
                List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                SetCellsReserved(robot, reservedList);
            }
        }

        /// <summary>
        /// Ha a robot polc alatt áll attól függően, hogy a polc már a roboton van-e , és van- még a polcon árú, cselekszik a polccal
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>


        void ShelfAction(Robot robot)
        {
            //melyik polc is az amin a robot van?
            Shelf shelfInCell = new Shelf(-1, -1, null);
            foreach (Shelf shelf in ItemedShelfs)
            { 
                if(shelf.CurrentPositionX == robot.PositionX && shelf.CurrentPositionY == robot.PositionY)
                {
                    shelfInCell = shelf;
                }
            }
          
            //ha rajta van a polc és van a polcon árú
            if(robot.ShelfOnIt!=null && robot.ShelfOnIt.Targets.Count != 0)
            {
                Destination nextDestination = new Destination(-1, -1, -1);
                foreach (Destination destination in Destinations)
                {
                    if (destination.Item == robot.ShelfOnIt.Targets[0])
                    {
                        nextDestination = destination;
                    }
                }
                BFS bfs = new BFS(storage, destinations, itemedShelfs,storageReservation, robot, true);
                bfs.setField(Field.DESTINATION);
                List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                SetCellsReserved(robot, reservedList);
            }
            //ha rajta van a polc és nincs a polcon árú
            else if (robot.ShelfOnIt != null && robot.ShelfOnIt.Targets.Count == 0)
            {
                itemedShelfs.Remove(robot.ShelfOnIt);
                robot.PutDown();
                result.Energy[robots.IndexOf(robot)]++;
            }
            //ha nincs rajta van a polc és nincs a polcon árú
            else if (robot.ShelfOnIt == null && !ItemedShelfs.Contains(shelfInCell))
            {
                NewTask(robot);
            }
            //ha nincs rajta van a polc és van a polcon árú
            else if (robot.ShelfOnIt == null && ItemedShelfs.Contains(shelfInCell))
            {
                robot.PickUp(shelfInCell);
                result.Energy[robots.IndexOf(robot)]++;

            }
        }
        /// <summary>
        ///Összeadja a robotnként felhasznált energiát
        /// </summary>
        public void SumEnergy()
        {
            foreach(int energy in Result.Energy)
            {
                Result.EnergySum += energy;
            }
        }

        /// <summary>
        /// Ha a robot célállomáson van és nincs még target a polcon elküldi a polc helyére, ha még van rajta de nem a célállomásé a következő célállomáshoz küldi, ha pedig a célállomásé akkor leadja azt.
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>

        void DestinationAction(Robot robot)
        {
            if(robot.ShelfOnIt.Targets.Count != 0)
            {
                Destination destinationInCell = new Destination(-1,-1,-1);
                foreach(Destination destination in Destinations)
                {
                    if(destination.PositionX == robot.PositionX && destination.PositionY == robot.PositionY)
                    {
                        destinationInCell = destination;
                    }
                }

                if (robot.ShelfOnIt.Targets.Contains(destinationInCell.Item))
                {
                    robot.Give(destinationInCell.Item);
                }
                else if(!robot.ShelfOnIt.Targets.Contains(destinationInCell.Item) && robot.WaitingCounter < 8)
                {
                    Destination nextDestination = new Destination(-1, -1, -1);
                    foreach (Destination destination in Destinations)
                    {
                        if (destination.Item == robot.ShelfOnIt.Targets[0])
                        {
                            nextDestination = destination;
                        }
                    }
                    BFS bfs = new BFS(storage, destinations, itemedShelfs, storageReservation, robot, true);
                    bfs.setField(Field.DESTINATION);
                    List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                    SetCellsReserved(robot, reservedList);
                }
                else
                {
                    BFS bfs = new BFS(storage, destinations, itemedShelfs, storageReservation, robot, true);
                    bfs.setField(Field.EMPTY);
                    List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                    SetCellsReserved(robot, reservedList);
                }
            }
            else
            {
                
                BFS bfs = new BFS(storage, destinations, itemedShelfs, storageReservation, robot, false);
                bfs.setEndPos(robot.ShelfOnIt.HomePositionX, robot.ShelfOnIt.HomePositionY);
                List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                SetCellsReserved(robot, reservedList);
            }
        }
        /// <summary>
        ///Polconkénti vizsgálat arra, hogy minden célállomás elérhető-e a polctól
        /// </summary>
        public bool AreNotUnreachableShelf()
        {
            for (int i = 0; i < storage.SizeX; i++)
            {
                for (int j = 0; j < storage.SizeY; j++)
                {
                    if (storage.Matrix[i, j] == Field.SHELF)
                    {
                        foreach(Destination destination in destinations)
                        {
                            Robot robot = new Robot(i, j, Direction.LEFT, MaxEnergy);
                            robot.ShelfOnIt = new Shelf(i,j,null);
                            BFS bfs = new BFS(storage, destinations, itemedShelfs, StorageReservation, robot, false);
                            bfs.setEndPos(destination.PositionX, destination.PositionY);
                            List<Tuple<int, int>> reservedList = bfs.FindNearestPath();
                            if (reservedList == null)
                            {
                                return false;
                            }
                        }
                        
                        
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Ha a robotnak a raktár méretéhez képest megfelelő a töltöttsége true-val tér vissza, egyébként false-al.
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>
        bool ChargeCheck(Robot robot)
        {
            if (robot.Energy >= 4 * (storage.SizeX + storage.SizeY) + 3 )
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Szélességi gráfkeresés, mátrixra átírva útvonal tárolással.
        /// </summary>
        /// <param name="robot">Beállítja a vizsgált robotot</param>
        /// <param name="reservedList">Megadja a bfs által genrált listát, amit át kell adni a robotnak, és a storageben is tárolni kell</param>

        //DE! lehetne akkor "polchely" mező is és akkor mindegy melyik polcot hova rakjuk vissza 
        void SetCellsReserved(Robot robot, List<Tuple<int, int>> reservedList)
        {
            if (reservedList != null)
            {
                foreach (Tuple<int, int> cell in reservedList)
                {
                    robot.ReservedFields.Add(new Tuple<int, int, Field>(cell.Item1, cell.Item2, storage.Matrix[cell.Item1, cell.Item2]));
                    storageReservation.Matrix[cell.Item1, cell.Item2] = Field.RESERVED;
                }
                robot.SetEndPos(reservedList[0].Item1, reservedList[0].Item2);
                MoveAction(robot);

            }
            else
            {
                robot.WaitingCounter++;
            }
        }
        #endregion
    }
}
