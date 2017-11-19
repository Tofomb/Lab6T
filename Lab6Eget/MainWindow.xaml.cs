using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;

namespace Lab6Eget
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        DateTime start;

        //
        public event Action<Chairs, Patron, Glases, ConcurrentQueue<Patron>> FindingEmptyChair;
        //
        Stopwatch stopwatch = new Stopwatch();
        public WaitingParameters WP = new WaitingParameters();
        Glases glases = new Glases();
        public int indexOrder;
        public static CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationToken ct = cts.Token;
        Chairs chairs = new Chairs();
        public int PatronsInThePub = 0;
        public bool openBar = false;
        public Patron guest = new Patron();
        public ConcurrentQueue<Patron> BartenderQueue = new ConcurrentQueue<Patron>();
        public ConcurrentQueue<Patron> ChairQueue = new ConcurrentQueue<Patron>();
        
        public MainWindow()

        {
            InitializeComponent();
           // BartenderQueue.Enqueue(guest);

        }

        public void PersonNameToPub(string namn)
        {
            Dispatcher.Invoke(() =>
            {
                indexOrder++;
                BouncerListBox.Items.Insert(0, indexOrder + "_ " + namn + " arrives at the pub");
                System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + indexOrder + "_ " + namn + " arrives at the pub");
                PatronsInThePub++;
                GuestLabel.Content = $"Guest in the pub: {PatronsInThePub.ToString()}";
            });
        }

        public void BartenderInteraction(Patron patron)
        {
            indexOrder++;
            BartenderQueue.Enqueue(patron);
            Dispatcher.Invoke(() =>
            {
                BartenderListBox.Items.Insert(0, indexOrder + "_ " + patron.patronName + " orders a beer");
                System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + indexOrder + "_ " + patron.patronName + " orders a beer");
            });

            ChairQueue.Enqueue(patron);

            FindingEmptyChair?.Invoke(chairs, patron, glases, ChairQueue);

        }


        public void SittingAndDrinking(Patron patron)
        {
            indexOrder++;
            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, indexOrder + "_ " + patron.patronName + " drinks a cold beer.");
                ChairLabel.Content = $"Empty Chairs: {chairs.NumberOfEmptyChairs.ToString()}";
            });
        }

        public void LeavingPub(Patron patron)
        {
            indexOrder++;
            PatronsInThePub--;
            chairs.NumberOfEmptyChairs++;

            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, indexOrder + "_ " + patron.patronName + " Leaves the Pub.");
                ChairLabel.Content = $"Empty Chairs: {chairs.NumberOfEmptyChairs.ToString()}";
                GuestLabel.Content = GuestLabel.Content = $"Guest in the pub: {PatronsInThePub.ToString()}";
            });

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimerDisplay.Text = Convert.ToString((DateTime.Now - start).Seconds);
        }

        private void LoadMainFrame(object sender, RoutedEventArgs e)
        {
            chairs.SetChairs(WP.getNumberOfEmptyChairs());
            glases.SetGlases(WP.getNumberOfGlassesOnTheShelf(), 0);
            GlasLable.Content = $"Glas on the shelf: {glases.NumberOfGlasesOnShelf.ToString()}";
            ChairLabel.Content = $"Empty Chairs: {chairs.NumberOfEmptyChairs.ToString()}";
            GuestLabel.Content = $"Guest in the pub: {PatronsInThePub.ToString()}";
            BartenderQueue.TryDequeue(out Patron m);
            

        }

        private void OpeningBar(object sender, RoutedEventArgs e)
        {
            Bouncer bouncer = new Bouncer(this);
            if (openBar == false)
            {
                indexOrder++;
                BouncerListBox.Items.Insert(0, indexOrder + "_ Worlds End Opens!");
                System.IO.File.WriteAllText(@"WorldsEnd.txt", indexOrder + "_ Worlds End Opens!");

                // clock
                timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 0),
                                            DispatcherPriority.Background,
                                            timer_Tick, Dispatcher.CurrentDispatcher);
                timer.IsEnabled = true;
                start = DateTime.Now;
                openBar = true;
                OpenBarButton.Content = "Close";

                Task enter = Task.Run(() =>
                {

                    bouncer.Arrival += PersonNameToPub;
                    
                    while (!ct.IsCancellationRequested && (DateTime.Now - start).Seconds < WP.getBarIsOpenFor()) 
                    {
                        if (WP.checkIfCharterTripWillArrive() && (DateTime.Now - start).Seconds >= 20)
                        {
                            WP.charterTripArrives();
                        };

                        bouncer.Work(WP);
                    }

                    // When closing Bouncer
                    Dispatcher.Invoke(() =>
                    {
                        indexOrder++;
                        BouncerListBox.Items.Insert(0, indexOrder + "_ Bouncer Goes Home.");
                    });
                    cts.Cancel();  // cancel thread
                    timer.IsEnabled = false;
                    openBar = false;
                    Dispatcher.Invoke(() =>
                    {
                        OpenBarButton.Content = "We are closed: Wait For Tomorrow";
                        OpenBarButton.IsEnabled = false;
                    });
                    //  stopwatch.Start();

                    // bouncer home clock
                    DispatcherTimer bTimer = new DispatcherTimer();
                    bTimer.IsEnabled = true;
                    start = DateTime.Now;
                });
            }
            else
            {
                cts.Cancel();  // cancel thread
                timer.IsEnabled = false;
                openBar = false;
                OpenBarButton.Content = "We are closed: Wait For Tomorrow";
                OpenBarButton.IsEnabled = false;
              //  stopwatch.Start();

                // bouncer home clock
                DispatcherTimer bTimer = new DispatcherTimer();
                bTimer.IsEnabled = true;
                start = DateTime.Now;

            }
            if (openBar == true)
            {
                Bartender bartender = new Bartender();
                Task working = Task.Run(() =>
                {
                    Dispatcher.Invoke(() => 
                    {
                        indexOrder++;
                        BartenderListBox.Items.Insert(0, indexOrder + "_ Bartender waits for customers.");
                    });
                    bartender.LookingForCleanGlas += TakingCleanGlas;
                    do
                    {
                        bartender.Work();
                    }
                    while (!BartenderQueue.IsEmpty || openBar == true || (DateTime.Now - start).Seconds < WP.getPatronWalkingTime()/1000);
                    Dispatcher.Invoke(() =>
                    {
                        indexOrder++;
                        BartenderListBox.Items.Insert(0, indexOrder + "_ Bartender goes home");
                    });
                });
            }
            if (openBar == true)
            {
                Waitress waitress = new Waitress();

                Task WorkingWaitress = Task.Run(() =>
                {
                    waitress.LeavingCleanGlas += WriteLeavingGlasesOnShelf;
                    waitress.LookingForDirtyGlas += WritePickingUpDirtyGlases;
                    do
                    {
                        waitress.Waitering(glases);
                    }
                    while (glases.NumberOfGlasesOnShelf < WP.getNumberOfGlassesOnTheShelf() || PatronsInThePub > 0 || openBar == true);
                    Dispatcher.Invoke(() =>
                    {
                        indexOrder++;
                        WaitressListBox.Items.Insert(0, indexOrder + "_ Waitress goes home");
                    });
                });
            }
        }

        private void TakingCleanGlas(int obj)
        {
            if (BartenderQueue.IsEmpty == false)
            {
                if (glases.NumberOfGlasesOnShelf > 0)
                {
                    glases.NumberOfGlasesOnShelf--;
                    indexOrder++;
                    BartenderQueue.TryDequeue(out Patron T);
                    Dispatcher.Invoke(() =>
                    {
                        GlasLable.Content = $"Glas on the shelf: {glases.NumberOfGlasesOnShelf.ToString()}";
                        BartenderListBox.Items.Insert(0, indexOrder + "_ Gives beer to " + T.patronName);
                        System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + indexOrder + "_ Gives beer to " + T.patronName);
                    });
                }
            }
        }

        private void WritePickingUpDirtyGlases(int glasInHand)
        {
            indexOrder++;
            Dispatcher.Invoke(() =>
            {
                string text = $"{indexOrder} Waitress picks up {glasInHand}, glas(es).";
                WaitressListBox.Items.Insert(0, text);
                System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + text);
            });

        }
        private void WriteLeavingGlasesOnShelf(int obj)
        {
            indexOrder++;
            Dispatcher.Invoke(() =>
            {
                string text = $"{indexOrder} Leaving Washed glas on shelf.";
                WaitressListBox.Items.Insert(0, text);
                System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + text);
                GlasLable.Content = $"Glas on the shelf: {glases.NumberOfGlasesOnShelf.ToString()}";
            });
        }
    }
}
