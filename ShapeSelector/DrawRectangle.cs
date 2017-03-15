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
    class DrawRectangle : IActionMode
    {
        CanvasModel model;
        MainWindow view;
        Point startPoint;
        Rectangle rectangle;

        public DrawRectangle(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
        }

        public void StartDragAction(Point p)
        { 
            startPoint = p;
        }

        public void DragAction(Point p)
        {
            rectangle = new Rectangle();
            Point start = new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y));
            rectangle.Width = Math.Abs(p.X - startPoint.X);
            rectangle.Height = Math.Abs(p.Y - startPoint.Y);
            rectangle.Stroke = Brushes.Black;
            view.DrawTempShape(rectangle, start);
        }

        public void StopDragAction(Point p)
        {
            view.RemoveTempShape();
            model.AddShape(rectangle, new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y)));
            view.RefreshCanvas();
        }

        public void DoubleClickAction(Point p)
        {
            throw new NotImplementedException();
        }

        public void ExitCanvasAction()
        {
            throw new NotImplementedException();
        }

        public void ClickOutsideCanvasAction()
        {
            throw new NotImplementedException();
        }
    }
}
