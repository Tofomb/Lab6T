using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace Lab6Eget
{

    public class Waitress
    {
        public event Action<int> LookingForDirtyGlas;
        public event Action<int> LeavingCleanGlas;
        public int NumberOfGlasesInTheHand = 0;

        public void Waitering(Glases glases)
        {


            if (glases.NumberOfEmptyGlases > 0)
            {
                while (glases.NumberOfEmptyGlases > 0)
                {
                    glases.NumberOfEmptyGlases--;
                    NumberOfGlasesInTheHand++;
                }

                LookingForDirtyGlas?.Invoke(NumberOfGlasesInTheHand);
                Thread.Sleep(2000);

                //   WaitingTable();
                while (NumberOfGlasesInTheHand > 0)
                {
                    NumberOfGlasesInTheHand--;
                    glases.NumberOfGlasesOnShelf++;
                }
                LeavingCleanGlas?.Invoke(0);
                Thread.Sleep(2000);
            }
        }
        public void WaitingTable()
        {
            LookingForDirtyGlas?.Invoke(NumberOfGlasesInTheHand);
        }

    }
}