﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ShapeSelector
{
    class CanvasModel
    {
        public Dictionary<Shape, Point> Shapes { get; private set; }
        public BitmapImage CurrentImage { get; private set; }
        public string CurrentImagePath { get; set; }

        public CanvasModel()
        {
            Shapes = new Dictionary<Shape, Point>();
            CurrentImage = null;
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

        public void LoadImage(BitmapImage img)
        {
            CurrentImage = img;
        }

        public void ClearShapes()
        {
            Shapes = new Dictionary<Shape, Point>();
        }
    }
}
