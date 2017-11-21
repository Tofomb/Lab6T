using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace Lab6Eget
{

    public class Patron
    {
        WaitingParameters wp = new WaitingParameters();
        public int oneCount = 0;
        Random random = new Random();
        public event Action<Patron> LeavingThePub;
        public event Action<Patron> OrderABeer;
        public event Action<Patron> DrinkingBeer;
        public string patronName { get; set; }
        bool boolvalue = true;

        public void patronAct(Patron patron)
        {
            OrderABeer?.Invoke(patron);
        }
        public void LookingForTable(Chairs chair, Patron patron, Glases glases, ConcurrentQueue<Patron> chairQueue)
        {
            Thread.Sleep(wp.getTimeForPatronToGoToTheTable());

            chairQueue.TryPeek(out Patron tester);


            if (tester != null)
            {
                if (chair.NumberOfEmptyChairs > 0 && patron.patronName == tester.patronName)
                {
                    chairQueue.TryDequeue(out Patron pong);
                    chair.NumberOfEmptyChairs--;
                    Thread.Sleep(wp.getTimePourBeer());
                    DrinkingBeer?.Invoke(patron);
                    // drinks beer
                    Thread.Sleep(wp.getTimeToDrinkTheBeer());
                    glases.NumberOfEmptyGlases++;
                    LeavingThePub?.Invoke(patron);
                }

            }

            if (boolvalue)
                while (true)
                {
                    boolvalue = false;
                    LookingForTable(chair, patron, glases, chairQueue);
                }

        }
    }
}