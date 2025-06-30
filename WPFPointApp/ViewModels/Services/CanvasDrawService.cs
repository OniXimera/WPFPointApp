using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFPointApp.Configuration;
using WPFPointApp.Models;

namespace WPFPointApp.ViewModels.Services
{
    internal class CanvasDrawService : IPointDrawService
    {
        public CanvasDrawService(Canvas canvas)
        {
            m_Canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

        public void DrawPoint(Point point)
        {
            var ellipse = new Ellipse
            {
                Width = AppConfiguration.SizePoint,
                Height = AppConfiguration.SizePoint,
                Fill = Brushes.Red,
                Stroke = Brushes.DarkRed,
                StrokeThickness = 1
            };

            var pointToCanvas = ConvertToCanvasCoordinates(point.X, point.Y);
            Canvas.SetLeft(ellipse, pointToCanvas.X - AppConfiguration.HalfSizePoint);
            Canvas.SetTop(ellipse, pointToCanvas.Y - AppConfiguration.HalfSizePoint);
            m_Canvas.Children.Add(ellipse);
        }

        private Point ConvertToCanvasCoordinates(double planeX, double planeY)
        {
            var canvasSize = GetCanvasSize();
            double x = ((planeX - AppConfiguration.PlaneMin) / AppConfiguration.PlaneRange) * canvasSize;
            double y = canvasSize - ((planeY - AppConfiguration.PlaneMin) / AppConfiguration.PlaneRange) * canvasSize;
            return new Point(x, y);
        }

        private double GetCanvasSize()
        {
            if(m_CanvasSize == default)
            {
                m_CanvasSize = m_Canvas.ActualHeight;
            }

            return m_CanvasSize;
        }

        private double m_CanvasSize;

        private readonly Canvas m_Canvas;
    }
}
