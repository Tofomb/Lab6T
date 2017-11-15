using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace Lab6Eget
{

    public class Patron
    {
        public event Action<Patron> LeavingThePub;
        public event Action<Patron> OrderABear;
        public event Action<Patron> DrinkingBeer;
        public string patronName { get; set; }

        public void patronAct(Patron patron)
        {
            OrderABear?.Invoke(patron);
        }
        public void LookingForTable(Chairs chair, Patron patron, Glases glases)
        {
            Thread.Sleep(10000);

            if (chair.NumberOfEmptyChairs > 0)
            {
                chair.NumberOfEmptyChairs--;
                DrinkingBeer?.Invoke(patron);
                // drinks beer
                Thread.Sleep(5000);
                glases.NumberOfEmptyGlases++;
                LeavingThePub?.Invoke(patron);
                
                
            }
        }

    }
}