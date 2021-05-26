using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class ResultDiary
    {
        private int steps;

        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }


        private List<int> energy;
        public List<int> Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        private int energySum;
        public int EnergySum
        {
            get { return energySum; }
            set { energySum = value; }
        }

        public ResultDiary()
        {
            this.energySum = 0;
            this.energy = new List<int>();
            this.steps = 0;
        }
    }
}
