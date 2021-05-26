using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Dock
    {
        #region Constructor
        /// <summary>
        /// Létrehoz egy Dock objektumot, X,Y helyen, ezek változtathatlanok. Az elérhetőségét alapértelmezetten igazra állítja.
        /// </summary>
        /// <param name="x">readonly X koordináta beállítása</param>
        /// <param name="y">readonly Y koordináta beállítása</param>
        public Dock(int x, int y)
        {
            this.positionX = x;
            this.positionY = y;
            this.Available = true;
        }
        #endregion

        #region Properties
        private readonly int positionX;

        public int PositionX
        {
            get { return positionX; }
        }

        private readonly int positionY;

        public int PositionY
        {
            get { return positionY; }
        }

        private bool available;

        public bool Available
        {
            get { return available; }
            set { available = value; }
        }


        #endregion
    }
}
