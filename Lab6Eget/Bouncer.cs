
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Lab6Eget
{
    public class Bouncer
    {
        MainWindow mw;
        public Bouncer(MainWindow mainwindow)
        {
            mw = mainwindow;
        }

    //    public void Bouncer(MainWindow mainwindow) { }
        public event Action<string> Arrival;
        public event Action<Patron> OrderABear;
        public int personCount = 0;
        public string GetRandomName()

        {
            Random r = new Random();
            List<string> namn = new List<string>();
            namn.Add("Jonas");
            namn.Add("Anders");
            namn.Add("Johanna");
            namn.Add("Daniel");
            namn.Add("Beatrice");
            namn.Add("Erik");
            namn.Add("Klara");
            namn.Add("Michel");
            namn.Add("David");
            namn.Add("Bosse");
            namn.Add("Konrad");

            int index = personCount;
            personCount++;
            if (personCount >= namn.Count)
            {
                personCount = 0;
            }
            //int index = r.Next(0, namn.Count);
            return namn[index];
        }

        public void Work()
        {
            //Task enter = Task.Run(() =>
          //  {
             //   while (true)
              //  {
                    EnteringBar();
                    Thread.Sleep(4500);
              //  }
          //  });
        }

        public void EnteringBar()
        {
            var patron = new Patron();
            patron.patronName = GetRandomName();

            Arrival?.Invoke(patron.patronName);
            Thread.Sleep(2000);
            OrderABear?.Invoke(patron);
            //Patron börjar jobba
            patron.patronAct();
        }

    }
}