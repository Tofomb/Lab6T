
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Lab6Eget
{
    public class Chairs
    {
        
        public int NumberOfEmptyChairs;
        public void SetChairs(int inNumberOfEmptyChairs)
        {
            NumberOfEmptyChairs = inNumberOfEmptyChairs;
           
        }
    }
}
