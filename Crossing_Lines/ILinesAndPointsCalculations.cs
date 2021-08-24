using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Crossing_Lines
{
    interface ILinesAndPointsCalculations
    {
        public double TakeY(int incom, Line[] markedZone);

        public double TakeX(int incom, Line[] markedZone);

        public void CheckLinesAndPoints(Line line, Line[] markedZone);

        public bool IsLineCrossing(Line[] edges, Line line);

        public bool IsPointInZone(Line[] lines, Point point);
    }
}
