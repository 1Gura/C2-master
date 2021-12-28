using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Zadacha
    {
        private int id { get; set; }
        public int Id
        {
            get
            {
                return Id;
            }

            set
            {
                if (value > 0)
                {
                    Id = value;
                }
                else
                {
                    Id = 0;
                }
            }
        }

        public double N { get; set; }

        public Zadacha(double N = 60)
        {
            this.N = N;
        }

    }
}
