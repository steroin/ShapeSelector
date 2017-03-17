using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ShapeSelector
{
    class Selection : IActionMode
    {
        CanvasModel model;
        MainWindow view;
        Point startPoint;
        Point shapeStartPoint;
        bool dragging;
        Shape currentShape;

        public Selection(CanvasModel m, MainWindow v)
        {
            model = m;
            view = v;
            dragging = false;
            currentShape = null;
        }

        public void DoubleClickAction(Point p)
        {

        }

        public void DragAction(Point p)
        {
            if (dragging)
            {
                double moveByX = p.X - startPoint.X;
                double moveByY = p.Y - startPoint.Y;
                view.MoveShape(currentShape, new Point(shapeStartPoint.X+moveByX, shapeStartPoint.Y+moveByY));
            }
            view.UpdateCoords((int)p.X, (int)p.Y);
        }

        public void ExitCanvasAction()
        {
            view.UpdateCoords(0, 0);
        }

        public void StartDragAction(Point p)
        {
            if (!dragging)
            {
                dragging = true;
                startPoint = p;
            }
        }

        public void StopDragAction(Point p)
        {
            if (dragging)
            {
                if (currentShape != null) model.MoveShape(currentShape, new Point(p.X-startPoint.X, p.Y-startPoint.Y));
                dragging = false;
                currentShape = null;
                view.RefreshCanvas();
            }
        }

        public void ClickWithinAnotherShapeAction(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(sender+"");
            currentShape = (Shape)sender;
            shapeStartPoint = new Point(Canvas.GetLeft(currentShape), Canvas.GetTop(currentShape));
        }

        public void UpdateModel(CanvasModel model)
        {
            this.model = model;
        }
    }
}
