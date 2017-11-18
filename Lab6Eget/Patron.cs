using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace Lab6Eget
{

    public class Patron
    {
        public int oneCount = 0;
        Random random = new Random();
        public event Action<Patron> LeavingThePub;
        public event Action<Patron> OrderABeer;
        public event Action<Patron> DrinkingBeer;
        // public event Action<Patron> ChairQueueChecker;
        public string patronName { get; set; }

        public void patronAct(Patron patron)
        {
            OrderABeer?.Invoke(patron);
        }
        public void LookingForTable(Chairs chair, Patron patron, Glases glases, ConcurrentQueue<Patron> chairQueue)
        {
            Thread.Sleep(5000);

            // Denna är jobbig
            //chairQueue.Enqueue(patron);
            //
            chairQueue.TryPeek(out Patron tester);

            if (tester != null)
            {
                if (chair.NumberOfEmptyChairs > 0 && patron.patronName == tester.patronName)
                {
                    chairQueue.TryDequeue(out Patron pong);
                    int DrinkingTime = random.Next(10000, 20000);
                    chair.NumberOfEmptyChairs--;
                    DrinkingBeer?.Invoke(patron);
                    // drinks beer
                    Thread.Sleep(DrinkingTime);
                    glases.NumberOfEmptyGlases++;

                    LeavingThePub?.Invoke(patron);
//
             /*       if (oneCount == 0)
                    {
                        oneCount++;
                        while (true)
                        {

                            LookingForTable(chair, patron, glases, chairQueue);
                        }
                    }*/
                    //
                }
            }

            /*       else
                   {

                       Thread.Sleep(5000);
                       LookingForTable(chair, patron, glases, chairQueue);

                   }*/


        }
    }
}