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
using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;


namespace SpotifyOverlay
{
    public class Scraper
    {
        public string GetSpotifyTrackInfo()
        {
            var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

            if (proc == null)
            {
                return "Spotify is not running!";
            }

            if (string.Equals(proc.MainWindowTitle, "Spotify", StringComparison.InvariantCultureIgnoreCase))
            {
                return "No track is playing";
            }

            return proc.MainWindowTitle;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.Width;



            var timer = new Timer(1000);
            var scraper = new Scraper();

            timer.Elapsed += (sender, elapsedArgs) =>
            {
                string title = scraper.GetSpotifyTrackInfo();
                if (title == "No track is playing")
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Label, string>(SetValue), ArtistLabel, "");
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Label, string>(SetValue), SongLabel, "");
                }
                else
                {
                    string Artist = title.Substring(0, title.IndexOf("-"));
                    string Song = title.Substring(title.IndexOf("-") + 2);
                    Console.WriteLine(title);

                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Label, string>(SetValue), ArtistLabel, Artist);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<Label, string>(SetValue), SongLabel, Song);
                }

            };

            timer.Start();
            Console.ReadLine();

        }


        private static void SetValue(Label label, string title)
        {
            label.Content = title;
        }

        private void Window_Deactivated_1(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }

}
