using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeSelector
{
    class DrawEllipse : IActionMode
    {
        CanvasModel model;
        MainWindow view;
        Point startPoint;
        Ellipse ellipse;

        public DrawEllipse(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
        }
        public void ClickOutsideCanvasAction()
        {
            throw new NotImplementedException();
        }

        public void DoubleClickAction(Point p)
        {
            throw new NotImplementedException();
        }

        public void DragAction(Point p)
        {
            ellipse = new Ellipse();
            Point start = new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y));
            ellipse.Width = Math.Abs(p.X - startPoint.X);
            ellipse.Height = Math.Abs(p.Y - startPoint.Y);
            ellipse.Stroke = Brushes.Black;
            view.DrawTempShape(ellipse, start);            
        }

        public void ExitCanvasAction()
        {
            throw new NotImplementedException();
        }

        public void StartDragAction(Point p)
        {
            startPoint = p;
        }

        public void StopDragAction(Point p)
        {
            view.RemoveTempShape();
            model.AddShape(ellipse, new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y)));
            view.RefreshCanvas();
        }
    }
}
