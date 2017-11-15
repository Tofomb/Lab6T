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

namespace Lab6Eget
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        DateTime start;

        Glases glases = new Glases(8, 0);
        public int indexOrder;
        public static CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationToken ct = cts.Token;
        //public int glasOnShelf = 8;



        Chairs chairs = new Chairs(9);
        public int PatronsInThePub = 0;
        public bool openBar = false;
        public Patron guest = new Patron();
        public ConcurrentQueue<Patron> BartenderQueue = new ConcurrentQueue<Patron>();



        public MainWindow()

        {
            InitializeComponent();


            BartenderQueue.Enqueue(guest);
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
            Dispatcher.Invoke(() =>
            {
                indexOrder++;
                BartenderQueue.Enqueue(patron);
                BartenderListBox.Items.Insert(0, indexOrder + "_ " + patron.patronName + " orders a beer");
                System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + indexOrder + "_ " + patron.patronName + " orders a beer");

            });
            patron.LookingForTable(chairs, patron, glases);

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
            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, indexOrder + "_ " + patron.patronName + " Leavs the Pub.");
                ChairLabel.Content = $"Empty Chairs: {chairs.NumberOfEmptyChairs.ToString()}";
            });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimerDisplay.Text = Convert.ToString(DateTime.Now - start);
        }

        private void LoadMainFrame(object sender, RoutedEventArgs e)
        {

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

                    // Dispatcher.Invoke(() =>
                    //           {
                    bouncer.Arrival += PersonNameToPub;
                    //bouncer.OrderABear += BartenderInteraction;

                    //                });
                    while (!ct.IsCancellationRequested)
                    {
                        bouncer.Work();
                    }

                });

            }
            else
            {
                cts.Cancel();  // cancel thread
                timer.IsEnabled = false;
                openBar = false;
                OpenBarButton.Content = "Open";

            }
            Task working = Task.Run(() =>
            {

                Bartender bartender = new Bartender();
                while (true)
                {
                    bartender.Work();
                    Dispatcher.Invoke(() =>
                    {
                        bartender.LookingForCleanGlas += TakingCleanGlas;
                    });
                }
            });
        }

        private void TakingCleanGlas(int obj)
        {

            if (BartenderQueue.IsEmpty == false)
            {
                //     if (glasOnShelf > 0)
                if (glases.NumberOfGlasesOnShelf > 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        glases.NumberOfGlasesOnShelf--;
                            //glasOnShelf--;
                            indexOrder++;
                        BartenderQueue.TryDequeue(out Patron T);
                            //  GlasLable.Content = $"Glas on the shelf: {glasOnShelf.ToString()}";
                            GlasLable.Content = $"Glas on the shelf: {glases.NumberOfGlasesOnShelf.ToString()}";

                        BartenderListBox.Items.Insert(0, indexOrder + "_ Gives beer to " + T.patronName);
                        System.IO.File.AppendAllText(@"WorldsEnd.txt", "\n" + indexOrder + "_ Gives beer to " + T.patronName);
                    });

                }
            }
        }
    }
}
