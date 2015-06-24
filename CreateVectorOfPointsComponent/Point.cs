using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateVectorOfPointsComponent
{
    class Point
    {
        private int[] point;
       
        public Point(int[] point)
        {
           this.point = point;
        }

        public int[] _Point
        {
            get
            {
                return this.point;
            }
        }
    }
}
