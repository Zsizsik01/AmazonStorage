using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Robot
    {
        #region Constructor
        /// <summary>
        /// Létrehoz egy robot objektumot. Kötelező megadni a helyét, irányát és a maximum töltöttségi szintjét.
        /// </summary>
        /// <param name="x">X koordináta helye</param>
        /// <param name="y">Y koordináta helye</param>
        /// <param name="dir">Robot iránya</param>
        /// <param name="energy">readonly Maximális energia szint</param>
        public Robot(int x, int y, Direction dir, int energy)
        {
            this.PositionX = this.EndX = x;
            this.PositionY = this.EndY = y;
            this.Direction = dir;
            this.ShelfOnIt = null; //ezzel jelezzuk h nincs rajta
            this.ReservedFields = new List<Tuple<int, int, Field>>();
            this.energy = energy;
            this.waitingCounter = 0;
        }
        #endregion

        #region Properties

        private int positionX;

        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        private int waitingCounter;
        public int WaitingCounter
        {
            get { return waitingCounter; }
            set { waitingCounter = value; }
        }

        private int positionY;

        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        private int endX;

        public int EndX
        {
            get { return endX; }
            set { endX = value; }
        }

        private int endY;

        public int EndY
        {
            get { return endY; }
            set { endY = value; }
        }

        //private readonly int MaxEnergy;

        private  int energy;

        public int Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        private Shelf shelfOnIt;

        public Shelf ShelfOnIt
        {
            get { return shelfOnIt; }
            set { shelfOnIt = value; }
        }

        private Direction direction;

        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private List<Tuple<int, int, Field>> reservedFields;

        public List<Tuple<int, int, Field>> ReservedFields
        {
            get { return reservedFields; }
            set { reservedFields = value; }
        }

        #endregion


        #region Functions
        /// <summary>
        /// Átmozgatja a robotot az adott koordinátákra
        /// </summary>
        /// <param name="x">Beállítja az X koordinátát</param>
        /// <param name="y">Beállítja az Y koordinátát</param>
        public void Move(int x, int y) {
            this.PositionX = x;
            this.PositionY = y;
            this.energy = this.energy - 1;
        }
        /// <summary>
        /// A robot felvesz egy polcot és a megadott irányt veszi végpozicíónak. Azért kell paraméterben megadni, mert sem a robot sem a polc nem fér hozzá a célállomásokhoz.
        /// </summary>
        /// <param name="sh">A megadott polc amit felvett</param>
        /// <param name="destX">Az áru célállomás X koordinátája</param>
        /// <param name="destY">Az áru célállomás Y koordinátája</param>
        public void PickUp(Shelf sh) {
            this.ShelfOnIt = sh;
            this.energy = this.energy - 1;
        }
        /// <summary>
        /// Lerakja a polcot. (null = nincs rajta polc)
        /// </summary>
        public void PutDown() {
            this.ShelfOnIt = null;
            this.energy = this.energy - 1;
        }
        /// <summary>
        /// Robot töltését valósítja meg
        /// </summary>
        /// <param name="amount">Hozzáadja a mennyiséget a robot töltöttségéhez (negatív szám esetén csökkenést valósít meg)</param>
        public void Charge(int amount) {
            this.Energy = this.Energy + amount;
        }
        /// <summary>
        /// Meghívja a rajta lévő polc Delivered(item: int) függvényét, a megadott paraméter függvényében. A controller tudja hogy éppen melyik célállomáson áll, így könnyű ezt a paramétert átadnia.
        /// </summary>
        /// <param name="item">Áru amit éppen lead</param>
        public void Give(int item) {
            this.ShelfOnIt.Delivered(item);
        }
        /// <summary>
        /// Egyelőre ez a függvény úgy müködik, hogy a megadott irányba fordítja a robotot egyből.
        /// </summary>
        /// <param name="dir"> A megadott irányba fordul a robot</param>
        public void TurnAway(Direction dir)
        {
            this.Direction = dir;
            this.energy = this.energy - 1;
        }

        /// <summary>
        /// Beállítja a robot endX és endY változóit a megadott értékekre.
        /// </summary>
        /// <param name="x">ezzel lesz egyenlő az endX</param>
        /// <param name="y">ezzel lesz egyenlő az endY</param>
        public void SetEndPos(int x, int y)
        {
            this.EndX = x;
            this.EndY = y;
            waitingCounter = 0;
        }
        #endregion
    }
    /// <summary>
    /// Irányokat definiálja a következő sorrendben: jobb, le, bal, fel (0-3)
    /// </summary>
    public enum Direction
    {
        RIGHT = 0,
        DOWN = 1,
        LEFT = 2,
        UP = 3
    }
}
