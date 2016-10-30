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

namespace DDoS_Detector_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSpotAvailable(int left, int top, int size, int marginRadius)
        {
            double proposedCenterLeft = left + size / 2;
            double proposedCenterTop = top + size / 2;
            foreach (Ellipse el in canvas.Children.OfType<Ellipse>())
            {
                double checkedCenterLeft = Canvas.GetLeft(el);
                double checkedCenterTop = Canvas.GetTop(el);

                if(Math.Sqrt(Math.Pow(proposedCenterLeft - checkedCenterLeft, 2) + Math.Pow(proposedCenterTop - checkedCenterTop, 2)) < marginRadius)
                {
                    return false;
                }
            }
            return true;
        }

        public MainWindow()
        {
            InitializeComponent();

            Show();

            const int numberOfMachines = 100;
            const int machineSize = 30;
            const int marginRadius = 80;
            Random rnd = new Random();

            for (int i = 0; i < numberOfMachines; ++i)
            {
                var ellipse = new Ellipse();
                ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(200), (byte)rnd.Next(200), (byte)rnd.Next(200)));
                ellipse.Width = machineSize;
                ellipse.Height = machineSize;

                int left, top;
                int attempts = 0;
                do
                {
                    attempts += 1;
                    if (attempts >= 10000)
                    {
                        throw new Exception("Canvas is probably too small to draw that many machines!");
                    }

                    left = rnd.Next((int)canvas.ActualWidth - machineSize);
                    top = rnd.Next((int)canvas.ActualHeight - machineSize);
                }
                while (!isSpotAvailable(left, top, machineSize, marginRadius));

                Canvas.SetLeft(ellipse, left);
                Canvas.SetTop(ellipse, top);
                canvas.Children.Add(ellipse);
            }
        }
    }
}
