
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6Eget
{
    public class Bouncer
    {
        MainWindow mw;

        public Bouncer(MainWindow mainwindow)
        {
            mw = mainwindow;

        }


        public event Action<string> Arrival;
        public event Action<Chairs, Patron, Glases, ConcurrentQueue<Patron>> FindingEmptyChair;
        public int personCount = 0;
        public int HiddenCounter;
        public Random r = new Random();
        public string GetRandomName()


        {
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

            return "(" + HiddenCounter + ")" + " " + namn[index];
        }

        public void Work(WaitingParameters wp)
        {
            Thread.Sleep(wp.getDelayToNextPatron());
            int numberOFPatron = wp.getNumberOfPatrons();
            for (int ii = 0; ii < numberOFPatron; ii++)
            {
                EnteringBar(wp);
            }
        }

        public void EnteringBar(WaitingParameters wp)
        {

            Patron patron = new Patron();
            patron.LeavingThePub += mw.LeavingPub;
            patron.OrderABeer += mw.BartenderInteraction;
            patron.DrinkingBeer += mw.SittingAndDrinking;
            patron.patronName = GetRandomName();
            Arrival?.Invoke(patron.patronName);
            Task BeingPatron = Task.Run(() =>
            {
                Thread.Sleep(wp.getTimeForPatronToGoToTheBar());
                //
                mw.FindingEmptyChair += patron.LookingForTable;
                //
                patron.patronAct(patron);
            });
        }
    }
}