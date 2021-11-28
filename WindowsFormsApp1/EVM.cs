using System.Collections.Generic;
using System.Linq;


namespace WindowsFormsApp1
{
    class EVM
    {
        /// <summary>
        /// время смены терминала = 30
        /// </summary>
        public double time = 30;


        /// <summary>
        /// Буффер
        /// </summary>
        public List<Zadacha> stash = new List<Zadacha>();

        /// <summary>
        /// Количество символов оставшихся для обработки
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public int Work(Terminal terminal, int h)
        {
            return terminal.taskStash.FirstOrDefault().N -= terminal.M * h;
        }

        public void timeReset(double time = 30)
        {
            this.time = time;
        }

        public EVM(double t4 = 30)
        {
            this.time = t4;
        }
    }
}
