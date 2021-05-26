using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeTerror.Model
{
    public class QueueNode
    {
        private int posx;
        public int PosX
        {
            get { return posx; }
            set { posx = value; }
        }
        private int posy;
        public int PosY
        {
            get { return posy; }
            set { posy = value; }
        }

        private QueueNode parent;
        public QueueNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private int distance;
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public QueueNode(int posx, int posy, int distance, QueueNode parent)
        {
            this.posx = posx;
            this.posy = posy;
            this.parent = parent;
            this.distance = distance;
        }


    };
}
