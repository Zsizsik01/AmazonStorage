using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Destination
    {
        #region Constructor
        /// <summary>
        /// Létrehoz egy Célállomás objektumot X,Y helyeken, ezek változtathatatlan adatok és beállítja, hogy milyen árut lehet itt leadni.
        /// </summary>
        /// <param name="x">readonly X koordináta beállítása</param>
        /// <param name="y">readonly Y koordináta beállítása</param>
        /// <param name="item">beállítja milyen árut lehet leadni</param>
        public Destination(int x, int y, int item)
        {
            this.positionX = x;
            this.positionY = y;
            this.item = item;
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

        private readonly int item;

        public int Item
        {
            get { return item; }
        }

        #endregion
    }
}
