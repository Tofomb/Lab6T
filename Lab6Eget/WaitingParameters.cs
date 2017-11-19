using System;
namespace Lab6Eget
{


    // Class with all the parameters 
    // for all test cases
    public class WaitingParameters
    {
        Random random = new Random();

        // Default values

        // The bar is open for
        int barIsOpenFor = 120; // 120 s

        int numberOfGlassesOnTheShelf = 8; // default 8
        int numberOfEmptyChairs = 9; // default 9

        // Patron values
        int minDelayBetweenPatrons = 3000;      //  3 seconds
        int maxDelayBetweenPatrons = 10000;     // 10 seconds
        int timeForPatronToGoToTheBar = 1000;   //  1 second
        int timeForPatronToGoToTheTable = 4000; //  4 seconds
        int minTimeToDrinkTheBeer = 10000;      // 10 seconds
        int maxTimeToDrinkTheBeer = 20000;      // 20 seconds

        // Waitress Value
        int timeToCollectGlasses = 10000;       // 10 seconds
        int timetoWashGlasses = 15000;          // 15 seconds

        // Bartender Value
        int fetchGlas = 3000;
        int pouringBeer = 3000;

        bool patronStaysInTheBarTwiceTheTime = false;
        bool waitressWorksHarder = false;

        bool couplesNight = false;
        bool charterTripWillCome = true;
        bool charterTripHasArrived = false;

        // How long the bar is open
        public int getBarIsOpenFor()
        {
            return (barIsOpenFor);
        }

        // Time to the next patron
        public int getDelayToNextPatron()
        {
            return (random.Next(minDelayBetweenPatrons, maxDelayBetweenPatrons));
        }

        // Does the patron stay longer
        public bool isThePatronStayingLonger()
        {
            return patronStaysInTheBarTwiceTheTime;
        }

        // Does the waitress work harder
        public bool doesTheWaitressWorksHarder()
        {
            return waitressWorksHarder;
        }

        // Is it couples night
        public bool isItCouplesNight()
        {
            return couplesNight;
        }

        // Check if a charter trip will arrive
        public bool checkIfCharterTripWillArrive()
        {
            return charterTripWillCome;
        }

        // Call this method when the charter trip has arrived (after 20 seconds)
        public void charterTripArrives()
        {
            charterTripHasArrived = true;
            charterTripWillCome = false;
        }

        // Number of patrons to enter at the same time
        public int getNumberOfPatrons()
        {
            if (couplesNight)
                // Two at the same time
                return 2;
            else if (charterTripHasArrived)
            {
                // This will only happens once
                charterTripHasArrived = false;
                return 15;
            }
            else
            {
                // Normal case
                return 1;
            }
        }

        // Time for patron to approach thebar
        public int getTimeForPatronToGoToTheBar()
        {
            return (patronStaysInTheBarTwiceTheTime ? 2 * timeForPatronToGoToTheBar : timeForPatronToGoToTheBar);
        }

        // Time for patron to go to the table
        public int getTimeForPatronToGoToTheTable()
        {
            return (patronStaysInTheBarTwiceTheTime ? 2 * timeForPatronToGoToTheTable : timeForPatronToGoToTheTable);
        }

        // Time for patron to drink the beer
        public int getTimeToDrinkTheBeer()
        {
            int timeToDrinkTheBeer = random.Next(minTimeToDrinkTheBeer, maxTimeToDrinkTheBeer);
            return (patronStaysInTheBarTwiceTheTime ? 2 * timeToDrinkTheBeer : timeToDrinkTheBeer);
        }

        // Bartender Picking Glas
        public int getTimeToFetchGlas()
        {
            return fetchGlas;
        }

        // Bartender pours beer.
        public int getTimePourBeer()
        {
            return pouringBeer;
        }

        // Time to collect Dirty the glasses
        public int getTimeToCollectGlasses()
        {
            return (waitressWorksHarder ? timeToCollectGlasses / 2 : timeToCollectGlasses);
        }

        // Time to wash the glasses
        public int getTimeToWashGlasses()
        {
            return (waitressWorksHarder ? timetoWashGlasses / 2 : timetoWashGlasses);
        }

        // Number of glasses on the shelf
        public int getNumberOfGlassesOnTheShelf()
        {
            return (numberOfGlassesOnTheShelf);
        }

        // Number of empty chairs
        public int getNumberOfEmptyChairs()
        {
            return (numberOfEmptyChairs);
        }
        // Time for maxvalue, Patron valks to the pub after closing time
        public int getPatronWalkingTime()
        {
            return (maxDelayBetweenPatrons);
        }
    }
}
