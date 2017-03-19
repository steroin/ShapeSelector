using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShapeSelector
{
    static class FileManager
    {
        public static void SaveToFile(string fileName, string imageFileName, CanvasModel model)
        {
            string[] content = new string[model.Shapes.Count+1];
            content[0] = imageFileName;
            int count = 1;

            foreach(Shape s in model.Shapes.Keys)
            {
                if (s is Ellipse)
                {
                    Ellipse el = (Ellipse)s;
                    content[count] = "0;" + model.Shapes[s].X + ";" + model.Shapes[s].Y+";"+el.Width+";"+el.Height;
                }
                else if (s is Rectangle)
                {
                    Rectangle rec = (Rectangle)s;
                    content[count] = "1;" + model.Shapes[s].X + ";" + model.Shapes[s].Y + ";" + rec.Width + ";" + rec.Height;
                }
                else if (s is Polygon)
                {
                    Polygon pol = (Polygon)s;
                    content[count] += "2;" + model.Shapes[s].X + ";" + model.Shapes[s].Y+";";
                    foreach (Point p in pol.Points)
                    {
                        content[count] += p.X + ";" + p.Y + ";";
                    } 
                }
                count++;
            }
            File.WriteAllLines(fileName, content);
        }
        
        public static CanvasModel ReadFromFile(string fileName)
        {
            CanvasModel model = new CanvasModel();

            string[] content = File.ReadAllLines(fileName);
            model.CurrentImagePath = content[0];
            try
            {
                model.LoadImage(new BitmapImage(new Uri(content[0])));
            }
            catch(FileNotFoundException e)
            {
                throw new BindedImageFileNotFoundException(content[0]);
            }

            for(int i=1;i<content.Length;i++)
            {
                string[] tempArr = content[i].Split(';');
                Shape s = null;
                Point p = new Point();

                if(tempArr[0]=="0")
                {
                    s = new Ellipse();
                    p = new Point(double.Parse(tempArr[1]), double.Parse(tempArr[2]));
                    s.Width = double.Parse(tempArr[3]);
                    s.Height = double.Parse(tempArr[4]);
                }
                else if (tempArr[0] == "1")
                {
                    s = new Rectangle();
                    p = new Point(double.Parse(tempArr[1]), double.Parse(tempArr[2]));
                    s.Width = double.Parse(tempArr[3]);
                    s.Height = double.Parse(tempArr[4]);
                }
                else if (tempArr[0] == "2")
                {
                    s = new Polygon();
                    Polygon pol = (Polygon)s;
                    p = new Point(double.Parse(tempArr[1]), double.Parse(tempArr[2]));
                    
                    for(int j=3;j<tempArr.Length;j+=2)
                    {
                        if (!string.IsNullOrEmpty(tempArr[j]))
                            pol.Points.Add(new Point(double.Parse(tempArr[j]), double.Parse(tempArr[j+1])));
                    }
                }

                if(s!=null)model.AddShape(s, p);

            }
            return model;
        }
    }
    
}
