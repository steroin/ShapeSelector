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
    class DrawPolygon : IActionMode
    {
        CanvasModel model;
        MainWindow view;
        List<Point> points;
        bool dragging;

        public DrawPolygon(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
            points = new List<Point>();
            dragging = false;
        }

        public void DoubleClickAction(Point p)
        {
            if(dragging && points.Count>1)
            {
                dragging = false;
                double distX = p.X - points[0].X;
                double distY = p.Y - points[0].Y;
                double dist = Math.Sqrt(distX*distX + distY*distY);

                if(dist>3)points.Add(p);
                Polygon polygon = new Polygon();
                foreach (Point point in points) polygon.Points.Add(point);
                points.RemoveAll(x => true);
                view.RemoveTempShape();
                model.AddShape(polygon, new Point(0,0));
                view.RefreshCanvas();              
            }
        }

        public void DragAction(Point p)
        {
            if (dragging)
            {
                Polyline polyline = new Polyline();
                foreach (Point point in points) polyline.Points.Add(point);
                polyline.Points.Add(p);
                polyline.Stroke = Brushes.Black;
                view.DrawTempShape(polyline, new Point(0, 0));
            }
            view.UpdateCoords((int) p.X, (int)p.Y);
        }

        public void ExitCanvasAction()
        {
            view.UpdateCoords(0, 0);
        }

        public void StartDragAction(Point p)
        {
            points.Add(p);
            dragging = true;            
        }

        public void StopDragAction(Point p)
        {
        }

        public void ClickWithinAnotherShapeAction(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
