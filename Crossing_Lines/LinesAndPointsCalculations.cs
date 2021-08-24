using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Crossing_Lines
{
    class LinesAndPointsCalculations : ILinesAndPointsCalculations
    {
        public double TakeX(int incom, Line[] markedZone)
        {
            double result = 0;
            if (markedZone != null)
            {
                List<double> allX = new List<double>();
                foreach (var line in markedZone)
                {
                    allX.Add(line.X1);
                    allX.Add(line.X2);
                }
                switch (incom)
                {
                    case 1:
                        result = allX.Min();
                        break;
                    case 2:
                        result = allX.Max();
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            return result;
        }

        public double TakeY(int incom, Line[] markedZone)
        {
            double result = 0;
            if (markedZone != null)
            {
                List<double> allY = new List<double>();
                foreach (var line in markedZone)
                {
                    allY.Add(line.Y1);
                    allY.Add(line.Y2);
                }
                switch (incom)
                {
                    case 1:
                        result = allY.Min();
                        break;
                    case 2:
                        result = allY.Max();
                        break;
                    default:
                        result = 0;
                        break;
                }
            }
            return result;
        }

        public void CheckLinesAndPoints(Line line, Line[] markedZone)
        {
            Point start = new Point(line.X1, line.Y1);
            Point end = new Point(line.X2, line.Y2);

            bool isStartInZone = IsPointInZone(markedZone, start);
            bool isEndInZone = IsPointInZone(markedZone, end);
            bool isCrossing = IsLineCrossing(markedZone, line);

            if (isStartInZone || isEndInZone)
            {
                line.Stroke = Brushes.Red;
            }
            if (isCrossing)
            {
                line.Stroke = Brushes.Red;
            }
        }

        public bool IsLineCrossing(Line[] edges, Line line)
        {
            foreach (Line edge in edges)
            {
                double denom = ((edge.X2 - edge.X1) * (line.Y2 - line.Y1)) - ((edge.Y2 - edge.Y1) * (line.X2 - line.X1));

                if (denom == 0)
                {
                    continue;
                }

                double numer = ((edge.Y1 - line.Y1) * (line.X2 - line.X1)) - ((edge.X1 - line.X1) * (line.Y2 - line.Y1));

                double r = numer / denom;

                double numer2 = ((edge.Y1 - line.Y1) * (edge.X2 - edge.X1)) - ((edge.X1 - line.X1) * (edge.Y2 - edge.Y1));

                double s = numer2 / denom;

                if ((r < 0 || r > 1) || (s < 0 || s > 1))
                {
                    continue;
                }
                return true;
            }

            return false;
        }

        public bool IsPointInZone(Line[] lines, Point point)
        {
            bool isInZone = true;
            foreach (Line line in lines)
            {
                double startPoint = (line.X2 - line.X1) * (point.Y - line.Y1) -
                                    (point.X - line.X1) * (line.Y2 - line.Y1);
                if (startPoint < 0)
                {
                    isInZone = false;
                }
            }
            return isInZone;
        }
    }
}
