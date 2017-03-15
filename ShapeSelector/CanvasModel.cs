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

    }
}
