using System;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab6Eget
{
    public class Bartender
    {
        public event Action<int> LookingForCleanGlas;
        public void Work()
        {
            LookingForGlas(0);
        }

        private void LookingForGlas(int ob)
        {
            Thread.Sleep(4000);
            LookingForCleanGlas?.Invoke(0);
        }
    }
}