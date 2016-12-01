using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DDoS_Detector_Simulator
{
    public class Host
    {
        public const int machineSize = 20;
        public const int marginRadius = 20;

        public bool infected = false;
        public double probabilityOfStartingConnection = 0.05;
        public double probabilityOfInfectingOtherHost = 0.1;
        public double probabilityOfAttackingTarget = 0.5;
        public List<Host> responseTargets = new List<Host>();

        public int responseLimit;

        public int inputInCurrentTimeFrame = 0;
        public int outputInCurrentTimeFrame = 0;

        public Ellipse ellipse;

        static Random rnd = new Random();

        public Host(MainWindow window, bool zombie = false)
        {
            ellipse = new Ellipse();
            Canvas.SetZIndex(ellipse, 10);

            setInfected(zombie);

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

                left = rnd.Next((int)window.canvas.ActualWidth - machineSize);
                top = rnd.Next((int)window.canvas.ActualHeight - machineSize);
            }
            while (!window.isSpotAvailable(left, top, machineSize, marginRadius));

            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
        }

        public void resetLimit()
        {
            responseLimit = 5;
        }

        public void setInfected(bool state)
        {
            if(state)
            {
                ellipse.Fill = new ImageBrush(new BitmapImage(new Uri(@"zombie.png", UriKind.Relative)));
                if(!infected)
                    MainWindow.infectedMachines++;
            }
            else
            {
                ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(200), (byte)rnd.Next(200), (byte)rnd.Next(200)));
            }

            infected = state;
        }

        public void receiveConnection(Connection c)
        {
            if (c.isResponse)
                return;
            responseTargets.Add(c.from);
        }

    }
}
