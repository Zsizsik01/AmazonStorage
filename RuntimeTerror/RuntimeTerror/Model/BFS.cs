using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class BFS
    {
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
        private Robot robot;
        public Robot Robot
        {
            get { return robot; }
            set { robot = value; }
        }
        private Field field;
        public Field Field
        {
            get { return field; }
            set { field = value; }
        }

        private bool moreEndPos;
        public bool MoreEndPos
        {
            get { return moreEndPos; }
            set { moreEndPos = value; }
        }


        private List<Shelf> itemedShelfs;

        public List<Shelf> ItemedShelfs
        {
            get { return itemedShelfs; }
            set { itemedShelfs = value; }
        }

        List<Destination> destinations;
        public List<Destination> Destinations
        {
            get { return destinations; }
            set { destinations = value; }
        }

        private List<Tuple<int,int>> endPositions;
        public List<Tuple<int, int>> EndPositiions
        {
            get { return endPositions; }
            set { endPositions = value; }
        }

        private int endx;
        public int EndX
        {
            get { return endx; }
            set { endx = value; }
        }
        private int endy;
        public int EndY
        {
            get { return endy; }
            set { endy = value; }
        }

        private List<Tuple<int, int>> shortestPath;
        public List<Tuple<int, int>> ShortestPath
        {
            get { return shortestPath; }
            set { shortestPath = value; }
        }

        private Queue<QueueNode> minQueue;
        public Queue<QueueNode> MinQueue
        {
            get { return minQueue; }
            set { minQueue = value; }
        }

        private int minDistanceWithReservedFields;
        public int MinDistanceWithReservedFields
        {
            get { return minDistanceWithReservedFields; }
            set { minDistanceWithReservedFields = value; }
        }

        private int minDistanceWithoutReservedFields;
        public int MinDistanceWithoutReservedFields
        {
            get { return minDistanceWithoutReservedFields; }
            set { minDistanceWithoutReservedFields = value; }
        }

        private int actDistance;
        public int ActDistance
        {
            get { return actDistance; }
            set { actDistance = value; }
        }

        public BFS(Storage storage,List<Destination> destinations, List<Shelf> itemedShelfs, Storage storageReservation, Robot robot, bool moreEndPos)
        {
            this.storage = storage;
            this.storageReservation = storageReservation;
            this.robot = robot;
            this.moreEndPos = moreEndPos;
            this.endPositions = new List<Tuple<int, int>>();
            this.itemedShelfs = itemedShelfs;
            this.destinations = destinations;
            shortestPath = new List<Tuple<int, int>>();
            minDistanceWithoutReservedFields = storage.SizeX * storage.SizeY;
            minDistanceWithReservedFields = storage.SizeX * storage.SizeY;
            this.minQueue = new Queue<QueueNode>();
            actDistance = -1;
            
        }

        /// <summary>
        /// Beállíta a célcellák indexét
        /// </summary>
        /// <param name="endx">beállítja a cél x indexét</param>
        /// <param name="endy">beállítja a cél y indexét</param>
        public void setEndPos(int endx, int endy)
        {
            this.endx = endx;
            this.endy = endy;

        }
        /// <summary>
        /// Beállítja a célmezőtípust
        /// </summary>
        /// <param name="field">beállítja a keresett mezőtípust</param>
        public void setField(Field field)
        {
            this.field = field;
        }

        /// <summary>
        /// ellenőrzi hogy a kívánt lépés mátrixon belül van-e
        /// </summary>
        /// <param name="row">beállítja a sor indexét</param>
        /// <param name="col">beállítja az oszlop indexét</param>
        private bool isValid(int row, int col)
        {
            return (row >= 0) && (row < storage.SizeX) &&
                   (col >= 0) && (col < storage.SizeY);
        }

        // 4 szomszádos cellához használt indexek
        static int[] rowNum = { -1, 0, 0, 1 };
        static int[] colNum = { 0, -1, 1, 0 };

        /// <summary>
        /// attól függően hogy több célállomás van-e legenerál egy céllistát
        /// </summary>
        private void MakePathList()
        {
            if(moreEndPos)
            {
                for (int i = 0; i < storage.SizeX; i++)
                {
                    for (int j = 0; j < storage.SizeY; j++)
                    {
                        if (storage.Matrix[i, j] == field)
                        {
                            if(field == Field.EMPTY || field== Field.DOCK || (field == Field.SHELF && ItemInSHelf(i, j)) || (field == Field.DESTINATION && robot.ShelfOnIt.Targets.Contains(DestinationNum(i,j))))
                            endPositions.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }
            }
            else endPositions.Add(new Tuple<int, int>(endx,endy));
        }


        private bool ItemInSHelf(int posx, int posy)
        {
            foreach(Shelf shelf in itemedShelfs)
            {
                if(shelf.HomePositionX == posx && shelf.HomePositionY == posy)
                {
                    return true;
                } 
            }
            return false;
        }

        private int DestinationNum(int posx, int posy)
        {
            foreach (Destination destination in destinations)
            {
                if (destination.PositionX == posx && destination.PositionY == posy)
                {
                    return destinations.IndexOf(destination);
                }
            }
            return -1;
        }
        /// <summary>
        /// Megkeresi egy célhoz a legrövidebb utat
        /// </summary>
        /// <param name="x">beállítja a cél x indexét</param>
        /// <param name="y">beállítja a cél y indexét</param>
        private Queue<QueueNode> FindPath(int x, int y, bool resrvedAllowed)
        {
            

            //el lehet e jutni oda(nem foglalt-e már?)
            if (storageReservation.Matrix[x, y] == Field.RESERVED && !resrvedAllowed)
            {
                actDistance = -1;
                return null;
            }

            bool[,] visited = new bool[storage.SizeX, storage.SizeY];

            // jelöljük a már látogatott cellát
            visited[robot.PositionX, robot.PositionY] = true;

            // veremben tároljuk a csúcsokat
            Queue<QueueNode> queue = new Queue<QueueNode>();


            // a kezdő csúcs távolsága 0, szülője null
            QueueNode s = new QueueNode(robot.PositionX, robot.PositionY, 0, null);
            queue.Enqueue(s); // berakjuk a verembe a kezdő csúcsot

            // BFS algoritmus alkalmazása
            while (queue.Count != 0)
            {
                QueueNode curr = queue.Peek();
                int posX = curr.PosX;
                int posY = curr.PosY;
                //Point pt = curr.pt;

                // ha elértük a cél cellát vége
                if (posX == x && posY == y)
                {
                    actDistance = curr.Distance;
                    return queue;
                }

                // különben az elsőt kivesszük, és a következőt berakjuk

                queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int row = posX + rowNum[i];
                    int col = posY + colNum[i];


                    // ha a következő cella valid és még nem látogattuk berakjuk
                    if (!resrvedAllowed && isValid(row, col) &&
                        !visited[row, col] &&
                        storageReservation.Matrix[row, col] == Field.EMPTY &&
                        !(robot.ShelfOnIt != null && storage.Matrix[row, col] == Field.SHELF))

                    {
                        // megjelöljük látogatottként és eltáruljuk(pont,távolság,szülő)
                        visited[row, col] = true;
                        QueueNode Adjcell = new QueueNode(row, col, curr.Distance + 1, curr);
                        queue.Enqueue(Adjcell);

                    }

                    if (resrvedAllowed && isValid(row, col) &&
                        !visited[row, col] &&
                        !(robot.ShelfOnIt != null && storage.Matrix[row, col] == Field.SHELF) )
                    {
                        visited[row, col] = true;
                        QueueNode Adjcell = new QueueNode(row, col, curr.Distance + 1, curr);
                        queue.Enqueue(Adjcell);
                    }

                }
            }
            actDistance = -1;
            return null;
        }
        /// <summary>
        /// több cél küzöl kiválasztja a legközelebbit
        /// </summary>
        public List<Tuple<int, int>> FindNearestPath()
        {
            Queue<QueueNode> queueWithReservedFields = new Queue<QueueNode>();
            Queue<QueueNode> queueWithoutReservedFields = new Queue<QueueNode>();

            MakePathList();
            foreach (Tuple<int,int> position in endPositions)
            {
                /*queueWithoutReservedFields = FindPath(position.Item1, position.Item2, false);
                if (actDistance != -1 && actDistance <= minDistanceWithoutReservedFields)
                {
                    minQueue = queueWithoutReservedFields;
                    endx = position.Item1;
                    endy = position.Item2;
                }*/

                queueWithReservedFields = FindPath(position.Item1,position.Item2,true);
                if (actDistance != -1 && actDistance <= minDistanceWithReservedFields)
                {
                    minDistanceWithReservedFields = actDistance;
                }

                queueWithoutReservedFields = FindPath(position.Item1, position.Item2, false);
                if (actDistance != -1 && actDistance <= minDistanceWithoutReservedFields)
                {
                    minDistanceWithoutReservedFields = actDistance;
                    minQueue = queueWithoutReservedFields;
                    endx = position.Item1;
                    endy = position.Item2;
                }
            }
            //ha 2x rövidebb a foglalt út vár
            if(minDistanceWithReservedFields * 2 < minDistanceWithoutReservedFields)
            {
                return null;
            }
            //ha van út:
            if (minQueue.Count != 0)
            {
                //megfelelő útvonal kiválasztása a lehetséges útvonalakból
                while (minQueue.Peek().PosX != endx && minQueue.Peek().PosY != endy)
                {
                    minQueue.Dequeue();
                }
                QueueNode crawl = minQueue.Peek();
                List<QueueNode> path = new List<QueueNode>();
                while (crawl.Parent != null)
                {
                    ShortestPath.Add(new Tuple<int, int>(crawl.PosX, crawl.PosY));
                    crawl = crawl.Parent;
                }
                return ShortestPath;
            }
            else return null;
            
        }
    }
}
