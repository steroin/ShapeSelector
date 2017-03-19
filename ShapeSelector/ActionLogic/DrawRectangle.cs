using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        bool dragging;

        public DrawRectangle(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
            dragging = false;
        }

        public void StartDragAction(Point p)
        {
            if (!dragging)
            {
                dragging = true;
                startPoint = p;
            }
        }

        public void DragAction(Point p)
        {
            if (dragging)
            {
                rectangle = new Rectangle();
                Point start = new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y));
                rectangle.Width = Math.Abs(p.X - startPoint.X);
                rectangle.Height = Math.Abs(p.Y - startPoint.Y);
                rectangle.Stroke = Brushes.Black;
                view.DrawTempShape(rectangle, start);
            }
            view.UpdateCoords((int)p.X, (int)p.Y);
        }

        public void StopDragAction(Point p)
        {
            if (dragging)
            {
                dragging = false;
                view.RemoveTempShape();
                model.AddShape(rectangle, new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y)));
                view.RefreshCanvas();
                view.RefreshTable();
            }
        }

        public void DoubleClickAction(Point p)
        {

        }

        public void ExitCanvasAction()
        {
            view.UpdateCoords(0, 0);
        }

        public void ClickWithinAnotherShapeAction(object sender, MouseButtonEventArgs e)
        {
        }

        public void UpdateModel(CanvasModel model)
        {
            this.model = model;
        }
    }
}
