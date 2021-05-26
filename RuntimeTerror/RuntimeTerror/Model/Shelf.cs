using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class Shelf
    {
        #region Constructor
        /// <summary>
        /// Létrehoz egy polcot. Ehhez meg kell adni a pozicióját és az árukat amik rajta vannak.
        /// </summary>
        /// <param name="x">X koordinátája a mátrixban</param>
        /// <param name="y">Y koordinátája a mátrixban</param>
        /// <param name="targets">Áruk amik a polcon találhatóak</param>
        public Shelf(int x, int y, List<int> targets)
        {
            this.HomePositionX = this.CurrentPositionX = x;
            this.HomePositionY = this.CurrentPositionY= y;
            this.Targets = targets;
        }
        
        #endregion

        #region Properties
        private int homePositionX;

        public int HomePositionX
        {
            get { return homePositionX; }
            set { homePositionX = value; }
        }

        private int homePositionY;

        public int HomePositionY
        {
            get { return homePositionY; }
            set { homePositionY = value; }
        }

        private int currentPositionX;

        public int CurrentPositionX
        {
            get { return currentPositionX; }
            set { currentPositionX = value; }
        }

        private int currentPositionY;

        public int CurrentPositionY
        {
            get { return currentPositionY; }
            set { currentPositionY = value; }
        }

        private List<int> targets;

        public List<int> Targets
        {
            get { return targets; }
            set { targets = value; }
        }

        #endregion

        #region Functions
        public void Delivered(int item)
        {
            Targets.Remove(item);
        }
        #endregion
    }
}
