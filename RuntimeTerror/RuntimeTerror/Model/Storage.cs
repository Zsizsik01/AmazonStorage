using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Storage
    {
        #region Properties
        public Storage()
        {
        }

        private Field[,] matrix;
        //+
        private int sizeX;
        public int SizeX
        {
            get { return sizeX; }
            set { sizeX = value; }
        }
        private  int sizeY;

        public int SizeY
        {
            get { return sizeY; }
            set { sizeY = value; }
        }

        public Field[,] Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        #endregion

        #region Functions

        /// <summary>
        /// A mátrix egy adott pozícióján megváltoztatja a Field értékét.
        /// </summary>
        /// <param name="x">X tengely pozíciója</param>
        /// <param name="y">Y tengely pozíciója</param>
        /// <param name="value">Field érték, amit a átadunk a mátrixnak afott pozíción</param>
        public void ChangeObj(int x, int y, Field value)
        {
            Matrix[x, y] = value;
        }

        /// <summary>
        /// Jelenlegi méret alapján generál egy EMPTY mezőkkel rendelkező mátrixot.
        /// </summary>
        public void GenerateFields()
        {
            Matrix = new Field[SizeX, SizeY];

            for(int i = 0; i < SizeX; i++)
            {
                for(int j = 0; j < SizeY; j++)
                {
                    Matrix[i, j] = Field.EMPTY;
                }
            }
        }


        #endregion

    }
    /// <summary>
    /// Ez mező értéke, ezek lehetnek: üres, célállomás, dokkoló, polc, robot, robot-polccal
    /// </summary>
    public enum Field
    {
        EMPTY =0,
        DESTINATION = 1,
        DOCK = 2,
        SHELF = 3,
        ROBOT = 4,
        ROBOTWITHSHELF= 5,
        ROBOTWITHDOCK = 6,
        ROBOTWITHDESTINATION = 7,
        ROBOTWITHSHELFANDDESTINATION=8,
        RESERVED = 9
    }
    //TODO: megjelenítés szempontjából, többféle reserved mező kellene, vagy legyen inkább 2 mátrixunk?
}
