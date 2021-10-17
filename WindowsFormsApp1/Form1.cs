using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int sutki = 24 * 60 * 60;
        private int t1;
        private int t2;
        private int t3;
        private int t4;
        private int N;
        private int M;
        private int h;
        private int j;
        private string writePath = @"test.txt";
        private int countTask = 0;
        private int countStash = 0;
        private bool flagStop = false;
        private List<Terminal> terminals = new List<Terminal>();
        EVM evm = new EVM();
        int k = 1;
        int kTask = 0;
        private int delay = 0;
        int avarage = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (
                textBox9.Text.Length > 0 &&
                textBox10.Text.Length > 0 &&
                textBox11.Text.Length > 0 &&
                textBox12.Text.Length > 0 &&
                textBox13.Text.Length > 0 &&
                textBox14.Text.Length > 0 &&
                textBox15.Text.Length > 0
                )
                {
                    countStash = 0;
                    countTask = 0;
                    label16.Text = "В работе...";
                    textBox19.Text = "";
                    textBox17.Text = "";
                    timer1.Start();
                    timer2.Start();
                    await Task.Run(() =>
                    {
                        Start();
                    });
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Возникла ошибка " + err);
            }

        }

        private void Start()
        {
            try
            {
                t1 = Int32.Parse(textBox9.Text);
                t2 = Int32.Parse(textBox10.Text);
                t3 = Int32.Parse(textBox11.Text);
                t4 = Int32.Parse(textBox12.Text);
                N = Int32.Parse(textBox14.Text);
                M = Int32.Parse(textBox13.Text);
                h = Int32.Parse(textBox15.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Проверьте правильность введенных данных!");
            }


            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine("");
            }
            evm = new EVM(t4);
            terminals = new List<Terminal> { new Terminal(t1, M), new Terminal(t2, M), new Terminal(t3, M) };
            fileWrite("Время,с. Число_поставленных_задач Состояние_первого_терминала Состояние_второго_терминала Состояние_третьего_терминала" +
                    " Количество_решенных_задач Количество_задач_в_очереди");
            for (j = 3; j < sutki; j += h)
            {
                if (Int32.TryParse(textBox16.Text, out delay))
                {
                    Thread.Sleep(delay);
                }

                setTasks();
                if (terminals[0].Task != null || terminals[1].Task != null || terminals[2].Task != null)
                {

                    /// Тут идет обработка терминалом задачи, если задача успела выполнится,
                    /// то просто переходим к след. терминалу(или если ее нет),
                    /// иначе, если закончилось отведенное время поместим недоделанную задачу в СТЭШ
                    /// и перейдем к след. терминалу

                    int workOver = 0;
                    int timeOver = 0;
                    switch (k)
                    {
                        case 1:
                            if (terminals[0].Task != null)
                            {
                                workOver = (evm.Work(terminals[0], h));
                                timeOver = evm.time -= h;
                                if (workOver <= 0 || timeOver <= 0)
                                {
                                    evm.timeReset(t4);
                                    if (workOver <= 0)
                                    {
                                        countTask++;
                                    }
                                    else
                                    {
                                        evm.stash.Add(terminals[0].Task);
                                        countStash++;
                                    }
                                    terminals[0].Task = null;
                                    k = 2;
                                }
                            }
                            else
                            {
                                k = 2;
                                continue;

                            }
                            break;
                        case 2:
                            if (terminals[1].Task != null)
                            {
                                workOver = (evm.Work(terminals[1], h));
                                timeOver = evm.time -= h;
                                if (workOver <= 0 || timeOver <= 0)
                                {

                                    evm.timeReset(t4);
                                    if (workOver <= 0)
                                    {
                                        countTask++;
                                    }
                                    else
                                    {
                                        evm.stash.Add(terminals[1].Task);
                                        countStash++;
                                    }
                                    terminals[1].Task = null;
                                    k = 3;
                                }
                            }
                            else
                            {
                                k = 3;
                                continue;
                            }
                            break;
                        case 3:
                            if (terminals[2].Task != null)
                            {
                                workOver = (evm.Work(terminals[2], h));
                                timeOver = evm.time -= h;
                                if (workOver <= 0 || timeOver <= 0)
                                {

                                    evm.timeReset(t4);
                                    if (workOver <= 0)
                                    {
                                        countTask++;
                                    }
                                    else
                                    {
                                        evm.stash.Add(terminals[2].Task);
                                        countStash++;
                                    }
                                    terminals[2].Task = null;
                                    k = 1;
                                }
                            }
                            else
                            {
                                k = 1;
                                continue;

                            }
                            break;
                    }
                }
                else
                {
                    if (evm.stash.Count() > 0)
                    {
                        terminals[k - 1].Task = evm.stash.FirstOrDefault();
                    }
                }
                avarage += (countTask + countStash) / h;
                fileWrite();
            }
            if (j >= sutki)
            {
                flagStop = true;
                avarage = avarage / sutki;
                
                return;
            }
        }

        private void setTerminal(bool workOver)
        {
            evm.timeReset();
            if (!workOver)
            {
                evm.stash.Add(terminals[1].Task);
            }
            terminals[1].Task = null;
        }

        private void RadioBtnActivated(int k)
        {
            switch (k)
            {
                case 1:
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;

                    break;
                case 2:
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;

                    break;
                case 3:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    break;
            }
        }

        private void RadioRemove()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

        }
        private void setTasks()
        {
            var count = 0;
            foreach (Terminal terminal in terminals)
            {
                terminal.interval -= h;
                if (terminal.interval <= 0)
                {
                    count++;
                    if (terminal.Task == null)
                    {
                        terminal.Task = new Zadacha(N);
                    }
                    else
                    {
                        evm.stash.Add(new Zadacha(N));
                        countStash++;
                    }
                    terminal.interval = t1;
                }
            }
        }

        private void sendMessage(int kTask)
        {
            if (terminals.Count() > 0)
            {
                if (terminals[0].Task != null && terminals[0].interval != 0 && j % terminals[0].interval == 0)
                {
                    pictureBox8.Visible = true;
                }
                if (terminals[1].Task != null && terminals[1].interval != 0 && j % terminals[1].interval == 0)
                {
                    pictureBox9.Visible = true;
                }
                if (terminals[2].Task != null && terminals[2].interval != 0 && j % terminals[2].interval == 0)
                {
                    pictureBox10.Visible = true;
                }
            }

        }

        private void sendMessage()
        {
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
        }

        private void fileWrite(string str = "")
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true))
                {
                    if (str == "")
                    {
                        string state1 = k == 1 ? "Активен" : "Не_активен";
                        string state2 = k == 2 ? "Активен" : "Не_активен";
                        string state3 = k == 3 ? "Активен" : "Не_активен";
                        sw.WriteLine($"{j} {countTask + countStash} {state1} {state2} {state3} {countTask} {countStash}");
                    }
                    else
                    {
                        sw.WriteLine(str);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RadioBtnActivated(k);
            sendMessage(kTask);
            progressBar1.Value = (j * 100) / sutki;
            textBox19.Text = j.ToString();
            textBox7.Text = countTask.ToString();
            textBox8.Text = countStash.ToString();
            if (flagStop)
            {
                RadioRemove();
                sendMessage();
                timer1.Stop();
                textBox17.Text = avarage.ToString();
                fileWrite("Длина очереди неоконченных заданий = " + avarage);
                label16.Text = "Работа завершена успехом";
                MessageBox.Show("Работа завершена успехом");
                flagStop = false;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            sendMessage();
        }
    }
}
