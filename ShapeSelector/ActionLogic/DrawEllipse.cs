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
    class DrawEllipse : IActionMode
    {
        CanvasModel model;
        MainWindow view;
        Point startPoint;
        Ellipse ellipse;
        bool dragging;

        public DrawEllipse(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
            dragging = false;
        }

        public void DoubleClickAction(Point p)
        {

        }

        public void DragAction(Point p)
        {
            if (dragging)
            {
                ellipse = new Ellipse();
                Point start = new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y));
                ellipse.Width = Math.Abs(p.X - startPoint.X);
                ellipse.Height = Math.Abs(p.Y - startPoint.Y);
                ellipse.Stroke = Brushes.Black;
                view.DrawTempShape(ellipse, start);
            }
            view.UpdateCoords((int)p.X, (int)p.Y);
        }

        public void ExitCanvasAction()
        {
            view.UpdateCoords(0,0);
        }

        public void StartDragAction(Point p)
        {
            if(!dragging)
            {
                dragging = true;
                startPoint = p;
            }
        }

        public void StopDragAction(Point p)
        {
            if (dragging)
            {
                dragging = false;
                view.RemoveTempShape();
                model.AddShape(ellipse, new Point(Math.Min(p.X, startPoint.X), Math.Min(p.Y, startPoint.Y)));
                view.RefreshCanvas();
                view.RefreshTable();
            }
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
