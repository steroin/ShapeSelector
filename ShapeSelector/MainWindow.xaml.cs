using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShapeSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CanvasModel canvasModel;
        IActionMode currentMode;
        Shape tempShape;
        ObservableCollection<TableRow> tableData;
        public Shape Selected { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            canvasModel = new CanvasModel();
            currentMode = new Selection(canvasModel, this);
            tableData = new ObservableCollection<TableRow>();
            Selected = null;
        }

        public void RefreshCanvas()
        {
            canvas.Children.Clear();
            canvas.Background = new ImageBrush(canvasModel.currentImage);
            

            foreach (Shape s in canvasModel.Shapes.Keys)
            {
                s.Stroke = null;
                s.Opacity = 0.4;
                if (s == Selected) s.Stroke = Brushes.Black;
                s.Fill = GetCurrentColor();
                Canvas.SetTop(s, canvasModel.Shapes[s].Y);
                Canvas.SetLeft(s, canvasModel.Shapes[s].X);
                s.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                s.MouseLeftButtonDown += currentMode.ClickWithinAnotherShapeAction;
                s.MouseMove += canvas_MouseMove;
                s.MouseLeftButtonUp += canvas_MouseLeftButtonUp;
                canvas.Children.Add(s);                             
            }
        }

        public void RefreshTable()
        {
            int count = 1;
            tableData.Clear();

            foreach(Shape s in canvasModel.Shapes.Keys)
            {
                tableData.Add(new TableRow(count, getShapeName(s), getShapeDetails(s)));
                count++;
            }
            shapesTable.ItemsSource = tableData;
        }
        
        
        public void DrawTempShape(Shape s, Point pos)
        {
            if (tempShape != null)canvas.Children.Remove(tempShape);

            canvas.Children.Add(s);
            Canvas.SetTop(s, pos.Y);
            Canvas.SetLeft(s, pos.X);
            s.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
            s.MouseLeftButtonDown += currentMode.ClickWithinAnotherShapeAction;
            s.MouseMove += canvas_MouseMove;
            s.MouseLeftButtonUp += canvas_MouseLeftButtonUp;
            tempShape = s;
        }
        public void MoveShape(Shape s, Point point)
        {
            foreach (Shape shape in canvas.Children)
            {
                if(shape==s)
                {
                    Canvas.SetLeft(s, point.X);
                    Canvas.SetTop(s, point.Y);
                    break;
                }
            }
        }

        public void RemoveTempShape()
        {
            if (tempShape != null)
            {
                canvas.Children.Remove(tempShape);
                tempShape = null;
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (canvasModel.currentImage == null) return;
            Selected = null;
            if (e.ClickCount == 2) currentMode.DoubleClickAction(e.GetPosition(canvas));
            else currentMode.StartDragAction(e.GetPosition(canvas));           
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (canvasModel.currentImage == null) return;
            currentMode.StopDragAction(e.GetPosition(canvas));           
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvasModel.currentImage == null) return;
            currentMode.DragAction(e.GetPosition(canvas));
        }

        Brush GetCurrentColor()
        {
            Brush ret = null;
            switch (((ComboBoxItem)comboBox_ColorPicker.SelectedItem).Name)
            {
                case "cbitem_Blue":
                    ret = Brushes.Blue;
                    break;
                case "cbitem_Gray":
                    ret = Brushes.Gray;
                    break;
                case "cbitem_Green":
                    ret = Brushes.Green;
                    break;
                case "cbitem_Red":
                    ret = Brushes.Red;
                    break;
                case "cbitem_Yellow":
                    ret = Brushes.Yellow;
                    break;
            }
            return ret;
        }

        string getShapeName(Shape s)
        {
            string ret = "undefined";

            if (s is Ellipse) ret = "okrąg";
            else if (s is Rectangle) ret = "prostokąt";
            else if (s is Polygon) ret = "wielokąt";

            return ret;
        }

        string getShapeDetails(Shape s)
        {
            string ret = "";

            if (s is Ellipse)           
                ret = "p = [" + Canvas.GetLeft(s) + "," + Canvas.GetTop(s) + "], w = " + s.Width + ", h = " + s.Height;            
            else if (s is Rectangle)
                ret = "p = [" + Canvas.GetLeft(s) + "," + Canvas.GetTop(s) + "], w = " + s.Width + ", h = " + s.Height;
            else if (s is Polygon)
            {
                bool first = true;

                foreach(Point p in ((Polygon)s).Points)
                {
                    if (!first) ret += ", ";                  
                    else first = false;
                    string x = string.Format("{0,2}", p.X + Canvas.GetLeft(s));
                    string y = string.Format("{0,2}", p.Y + Canvas.GetTop(s));
                    ret += "[" + x + ";" + y + "]";
                }         
            }

            return ret;
        }

        private void button_Oval_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new DrawEllipse(canvasModel, this);
        }

        private void button_Rectangle_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new DrawRectangle(canvasModel, this);
        }

        private void button_Polygon_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new DrawPolygon(canvasModel, this);
        }

        private void button_Selection_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new Selection(canvasModel, this);
            RefreshCanvas();
        }

        private void comboBox_ColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(canvas!=null)RefreshCanvas();
        }

        public void UpdateCoords(int x, int y)
        {
            label_Coords.Content = "["+x+","+y+"]";
        }

        private void canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (canvasModel.currentImage == null) return;
            currentMode.ExitCanvasAction();
        }

        private void mitem_FileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                DefaultExt = ".jpg",
                Filter = "ShapeSelector shape template file (*.shps)|*.shps|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif"
            };

            bool? result = dialog.ShowDialog();

            if(result==true)
            {
                string fileName = dialog.FileName;
                string ext = dialog.FileName.Substring(dialog.FileName.Length - 5);

                if (ext == ".shps")
                {
                    try
                    {
                        canvasModel = FileManager.ReadFromFile(fileName);
                        canvas.Width = canvasModel.currentImage.Width;
                        canvas.Height = canvasModel.currentImage.Height;
                        currentMode.UpdateModel(canvasModel);
                    }
                    catch(BindedImageFileNotFoundException ex)
                    {
                        MessageBox.Show("Nie odnaleziono powiązanego pliku z obrazem: "+ex.FileName, 
                            "Nie znaleziono obrazu", 
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    var img = new BitmapImage(new Uri(dialog.FileName));
                    canvasModel.currentImagePath = dialog.FileName;
                    canvasModel.LoadImage(img);
                    canvasModel.ClearShapes();
                    canvas.Width = img.Width;
                    canvas.Height = img.Height;
                }

                RefreshCanvas();
                RefreshTable();
            }
        }

        private void mitem_FileClose_Click(object sender, RoutedEventArgs e)
        {
            canvasModel = new CanvasModel();
            canvas.Background = null;
            RefreshCanvas();
            RefreshTable();
        }

        private void mitem_FileSave_Click(object sender, RoutedEventArgs e)
        {
            if (canvasModel.currentImage == null)
            {
                MessageBox.Show("Nie znaleziono obrazu do zapisania.",
                               "Brak obrazu do zapisu",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "ShapeSelector shape template file (*.shps)|*.shps";

            if(dialog.ShowDialog()==true)
            {
                FileManager.SaveToFile(dialog.FileName, canvasModel.currentImagePath, canvasModel);
            }
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelectedShape();
            }
        }

        private void DeleteSelectedShape()
        {
            if (Selected != null)
            {
                canvasModel.Shapes.Remove(Selected);
                Selected = null;
                RefreshCanvas();
                RefreshTable();
            }
        }

        private void shapesTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = shapesTable.SelectedIndex;
            if (index >= 0)
            {
                if (Selected != null) Selected.Stroke = null;
                Selected = canvasModel.Shapes.ElementAt(index).Key;
                Selected.Stroke = Brushes.Black;
            }
        }
    }
}
