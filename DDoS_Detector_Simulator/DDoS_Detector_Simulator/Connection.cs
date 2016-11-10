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
        Host from, to;
        public DateTime beginTime, hideTime;

        public Line line;

        public Connection(Host from, Host to)
        {
            beginTime = DateTime.Now;
            hideTime = beginTime.AddSeconds(1);

            line = new Line();

            line.X1 = Canvas.GetLeft(from.ellipse) + Host.machineSize / 2;
            line.Y1 = Canvas.GetTop(from.ellipse) + Host.machineSize / 2;
            line.X2 = Canvas.GetLeft(to.ellipse) + Host.machineSize / 2;
            line.Y2 = Canvas.GetTop(to.ellipse) + Host.machineSize / 2;

            line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            line.StrokeThickness = 1;
        }
    }
}
