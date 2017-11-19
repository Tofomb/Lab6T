
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Lab6Eget
{

    public class Glases
    {
        public int NumberOfGlasesOnShelf;
        public int NumberOfEmptyGlases;
        public void SetGlases(int inNumberOfGlasesOnShelf, int inNumberOfEmptyGlases)
        {
            NumberOfGlasesOnShelf = inNumberOfGlasesOnShelf;
            NumberOfEmptyGlases = inNumberOfEmptyGlases;
        }



    }

}
