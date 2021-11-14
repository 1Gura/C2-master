using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Terminal
    {
        /// <summary>
        /// Время на обработку задачи в секундакх
        /// </summary>
        public int interval = 30;
        /// <summary>
        /// Количество символов обрабатываемых за секунду
        /// </summary>
        public int M = 3;


        /// <summary>
        /// Буффер
        /// </summary>
        public List<Zadacha> taskStash = new List<Zadacha>();


        public Terminal(int timeWork = 30, int M = 3)
        {
            this.interval = timeWork;
            this.M = M;
        }

    }
}