
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
    //    public event Action<Patron> OrderABear;
        public int personCount = 0;
        public int HiddenCounter;
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
            HiddenCounter++;
            if (personCount >= namn.Count)
            {
                personCount = 0;
            }
            //int index = r.Next(0, namn.Count);
            return "(" + HiddenCounter +  ")" + " " + namn[index];
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
            
            Patron patron = new Patron();
            patron.LeavingThePub += mw.LeavingPub;
            patron.OrderABear += mw.BartenderInteraction;
            patron.DrinkingBeer += mw.SittingAndDrinking;
            patron.patronName = GetRandomName();
            Arrival?.Invoke(patron.patronName);
            Task BeingPatron = Task.Run(() => 
            {
                Thread.Sleep(2000);
                patron.patronAct(patron);
            });


        }

    }
}