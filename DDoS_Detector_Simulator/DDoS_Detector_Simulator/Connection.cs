using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace DDoS_Detector_Simulator
{
    public class Connection
    {
        public Host from, to;
        public DateTime beginTime, hideTime;
        public bool isResponse;
        public Line line;

        public Connection(Host from, Host to, bool isResponse = false)
        {
            this.from = from;
            this.to = to;

            beginTime = DateTime.Now;
            hideTime = beginTime.AddMilliseconds(500);
            this.isResponse = isResponse;
            line = new Line();

            line.X1 = Canvas.GetLeft(from.ellipse) + Host.machineSize / 2;
            line.Y1 = Canvas.GetTop(from.ellipse) + Host.machineSize / 2;
            line.X2 = Canvas.GetLeft(to.ellipse) + Host.machineSize / 2;
            line.Y2 = Canvas.GetTop(to.ellipse) + Host.machineSize / 2;

            if (this.isResponse)
            {
                line.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                line.StrokeThickness = 3;
                Canvas.SetZIndex(line, -1);
            }
            else
            {
                line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                line.StrokeThickness = 1;
                Canvas.SetZIndex(line, 1);
            }
        }
    }
}
