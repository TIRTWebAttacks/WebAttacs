using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DDoS_Detector_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isSpotAvailable(int left, int top, int size, int marginRadius)
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

        public Thread t;

        public MainWindow()
        {
            InitializeComponent();

            Show();

            const int numberOfMachines = 20;

            //this.Hosts.Text = numberOfMachines.ToString();

            List<Host> hosts = new List<Host>();

            for (int i = 0; i < numberOfMachines; ++i)
            {
                var infected = i == 0;
                var host = new Host(this, infected);
                hosts.Add(host);
                canvas.Children.Add(host.ellipse);
            }

            Random rnd = new Random();
            List<Connection> connections = new List<Connection>();

            t = new Thread(() =>
            {
                while (true)
                {
                    foreach (var host in hosts)
                    {
                        double p = rnd.NextDouble();
                        if (p < host.probabilityOfStartingConnection)
                        {
                            Host to;
                            do
                            {
                                to = hosts[rnd.Next(hosts.Count)];
                            } while (host == to);
                            this.Dispatcher.BeginInvoke(new InvokeCreateConection(createConnection), host, to, connections);
                        }
                    }

                    foreach(var c in connections)
                    {
                        if(c.hideTime < DateTime.Now)
                        {
                            this.Dispatcher.BeginInvoke(new InvokeDelegate(clearConnection), c);
                        }
                    }

                    connections.RemoveAll(c => c.hideTime < DateTime.Now);

                    Thread.Sleep(100);
                }
            });
            t.Start();
        }

        public delegate void InvokeDelegate(Connection c);
        public void clearConnection(Connection c)
        {
            canvas.Children.Remove(c.line);
        }

        public delegate void InvokeCreateConection(Host host, Host to, ref List<Connection> connections);
        public void createConnection(Host host, Host to, ref List<Connection> connections)
        {
            Random rnd = new Random();

            var connection = new Connection(host, to);
            connections.Add(connection);
            canvas.Children.Add(connection.line);

            double pp = rnd.NextDouble();
            if (pp < host.probabilityOfInfectingOtherHost)
            {
                to.setInfected(true);
            }
        }

    }
}
