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

namespace Crossing_Lines
{

    public partial class MainWindow : Window
    {
        private Line[] markedZone = null;
        private Point firstPoint = new Point(-1, -1);
        private ILinesAndPointsCalculations calculation;

        public MainWindow()
        {
            calculation = new LinesAndPointsCalculations();
            InitializeComponent();
        }

        private void ShapeCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int lineCount = rand.Next(1, 10),
                startX = rand.Next(1, 20), startY = rand.Next(1, 20),
                x = startX, y = startY;
            for (int i = 0; i < lineCount; i++)
            {
                int width = Convert.ToInt32(MainCanvas.Width);
                int height = Convert.ToInt32(MainCanvas.Height);
                Line newLine = new Line();
                newLine.Stroke = Brushes.Black;
                newLine.X1 = x;
                newLine.Y1 = y;
                x = rand.Next(0, width);
                y = rand.Next(0, height);
                newLine.X2 = x;
                newLine.Y2 = y;
                newLine.Tag = "Shape";
                newLine.HorizontalAlignment = HorizontalAlignment.Left;
                newLine.VerticalAlignment = VerticalAlignment.Center;
                newLine.StrokeThickness = 2;
                MainCanvas.Children.Add(newLine);
            }
            Line lastLine = new Line();
            lastLine.Stroke = Brushes.Black;
            lastLine.X1 = x;
            lastLine.Y1 = y;
            lastLine.X2 = startX;
            lastLine.Y2 = startY;
            lastLine.Tag = "Shape";
            lastLine.HorizontalAlignment = HorizontalAlignment.Left;
            lastLine.VerticalAlignment = VerticalAlignment.Center;
            lastLine.StrokeThickness = 2;
            MainCanvas.Children.Add(lastLine);
        }

        private void LineCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            int lineCount = 0;
            Random rand = new Random();
            lineCount = rand.Next(1, 20);
            for (int i = 0; i < lineCount; i++)
            {
                int width = Convert.ToInt32(MainCanvas.Width);
                int height = Convert.ToInt32(MainCanvas.Height);
                Line newLine = new Line();
                newLine.Stroke = Brushes.LightSteelBlue;
                newLine.X1 = rand.Next(0, width);
                newLine.Y1 = rand.Next(0, height);
                newLine.X2 = rand.Next(0, width);
                newLine.Y2 = rand.Next(0, height);
                newLine.HorizontalAlignment = HorizontalAlignment.Left;
                newLine.VerticalAlignment = VerticalAlignment.Center;
                newLine.StrokeThickness = 2;
                newLine.Tag = "Line";
                MainCanvas.Children.Add(newLine);
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == MainCanvas)
            {
                firstPoint = e.GetPosition(MainCanvas);
                Shape wanted = null;
                foreach (Shape wantedI in MainCanvas.Children)
                {
                    if (wantedI.Name == "CheckZone")
                    {
                        wanted = wantedI;
                        break;
                    }
                }
                if (wanted != null)
                {
                    MainCanvas.Children.Remove(wanted);
                }
                ResetLines();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (firstPoint.X == -1)
            {
                return;
            }

            if (markedZone == null)
            {
                markedZone = new Line[4];

                for (int i = 0; i < 4; i++)
                {
                    markedZone[i] = new Line();
                    markedZone[i].Stroke = Brushes.Gray;
                    markedZone[i].StrokeThickness = 1;
                    markedZone[i].SnapsToDevicePixels = false;
                    markedZone[i].HorizontalAlignment = HorizontalAlignment.Left;
                    markedZone[i].VerticalAlignment = VerticalAlignment.Top;
                    markedZone[i].Tag = "Edge";

                    MainCanvas.Children.Add(markedZone[i]);
                }
            }

            double currX = e.GetPosition(MainCanvas).X;
            double currY = e.GetPosition(MainCanvas).Y;

            markedZone[0].X1 = firstPoint.X;
            markedZone[0].Y1 = firstPoint.Y;
            markedZone[0].X2 = currX;
            markedZone[0].Y2 = firstPoint.Y;

            markedZone[1].X1 = currX;
            markedZone[1].Y1 = firstPoint.Y;
            markedZone[1].X2 = currX;
            markedZone[1].Y2 = currY;

            markedZone[2].X1 = currX;
            markedZone[2].Y1 = currY;
            markedZone[2].X2 = firstPoint.X;
            markedZone[2].Y2 = currY;

            markedZone[3].X1 = firstPoint.X;
            markedZone[3].Y1 = currY;
            markedZone[3].X2 = firstPoint.X;
            markedZone[3].Y2 = firstPoint.Y;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (markedZone != null)
            {
                CreateZone();
                foreach (Shape shape in MainCanvas.Children)
                {
                    Line line = shape as Line;
                    if (line != null && line.Tag != "Edge")
                    {
                        calculation.CheckLinesAndPoints(line, markedZone);
                    }
                }
                ResetZone(); 
                firstPoint = new Point(-1, -1);
            }
        }

        private void CreateZone()
        {
            Rectangle checkZone = new Rectangle();
            checkZone.Stroke = Brushes.Gray;
            checkZone.Name = "CheckZone";
            checkZone.HorizontalAlignment = HorizontalAlignment.Left;
            checkZone.VerticalAlignment = VerticalAlignment.Center;
            checkZone.Height = calculation.TakeY(2, markedZone) - calculation.TakeY(1, markedZone);
            checkZone.Width = calculation.TakeX(2, markedZone) - calculation.TakeX(1, markedZone);
            Canvas.SetTop(checkZone, calculation.TakeY(1, markedZone));
            Canvas.SetLeft(checkZone, calculation.TakeX(1, markedZone));
            MainCanvas.Children.Add(checkZone);
        }

        private void ResetZone()
        {
            firstPoint = new Point(-1, -1);
            if (markedZone != null)
            {
                if (MainCanvas.Children.Contains(markedZone[0]))
                {
                    MainCanvas.Children.Remove(markedZone[0]);
                    MainCanvas.Children.Remove(markedZone[1]);
                    MainCanvas.Children.Remove(markedZone[2]);
                    MainCanvas.Children.Remove(markedZone[3]);
                }
                markedZone = null;
            }
        }

        private void ResetLines()
        {
            foreach (Shape shape in MainCanvas.Children)
            {
                Line line = shape as Line;
                if (line != null && line.Tag == "Shape")
                {
                    line.Stroke = Brushes.Black;
                }
                if (line != null && line.Tag == "Line")
                {
                    line.Stroke = Brushes.LightSteelBlue;
                }
            }
        }
    }
}
