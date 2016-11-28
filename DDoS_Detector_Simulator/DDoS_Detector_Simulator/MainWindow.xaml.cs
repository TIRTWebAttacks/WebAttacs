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
        DateTime ProgramStart = DateTime.Now;
        public static int infectedMachines = 0;
        const int numberOfMachines = 20;
        static List<Host> hosts = new List<Host>();
        public static List<Connection> connections = new List<Connection>();
        private bool RunProgram = false;

        public MainWindow()
        {
            InitializeComponent();

            Show();

            if (this.Start.IsPressed) RunProgram = !RunProgram;

            for (int i = 0; i < numberOfMachines; ++i)
            {
                var infected = i == 0;
                var host = new Host(this, infected);
                hosts.Add(host);
                canvas.Children.Add(host.ellipse);
            }


            Random rnd = new Random();
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
                            this.Dispatcher.Invoke(new InvokeCreateConection(createConnection), host, to, connections);
                        }
                    }

                    foreach(var c in connections)
                    {
                        if(c.hideTime < DateTime.Now)
                        {
                            this.Dispatcher.Invoke(new InvokeDelegate(clearConnection), c);
                        }
                    }

                    connections.RemoveAll(c => c.hideTime < DateTime.Now);
                    this.Dispatcher.BeginInvoke(new InvokeDrawSidePanel(drawInformationsOnSidePanel));
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

        public delegate void InvokeDrawSidePanel();
        public void drawInformationsOnSidePanel()
        {
            this.NumberOfHosts.Text = numberOfMachines.ToString();
            this.Time.Text = (DateTime.Now - ProgramStart).ToString();
            this.IfectedHosts.Text = infectedMachines.ToString();
            this.NumberOfConnectionsATM.Text = connections.Count.ToString();
            this.InfectionPercentage.Text = "0.05";
            this.ConnectionPercentage.Text = "0.05"; // Do poprawy
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            t.Abort();
        }
    }
}
