using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace ShapeSelector
{
    class CanvasModel
    {
        public Dictionary<Shape, Point> Shapes;

        public CanvasModel()
        {
            Shapes = new Dictionary<Shape, Point>();
        }

        public void AddShape(Shape s, Point p)
        {
            if(!Shapes.Keys.Contains(s))Shapes.Add(s, p);
        }


        public void MoveShape(Shape s, Point vector)
        {
            foreach(Shape shape in Shapes.Keys)
            {
                if (shape == s)
                {
                    double newX = Shapes[shape].X + vector.X;
                    double newY = Shapes[shape].Y + vector.Y;
                    Shapes[shape] = new Point(newX, newY);
                    break;
                }
            }
        }

        public IEnumerable<Shape> GetShapes()
        {
            foreach (Shape s in Shapes.Keys) yield return s;
        }

    }
}
