using System;
using System.Collections.Generic;
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
        bool dragging;
        public MainWindow()
        {
            InitializeComponent();
            canvasModel = new CanvasModel();
            currentMode = new DrawEllipse(canvasModel, this);
            dragging = false;
            canvas.Background = Brushes.Yellow;
            
        }

        public void RefreshCanvas()
        {
            canvas.Children.Clear();
           
            foreach(Shape s in canvasModel.Shapes.Keys)
            {
                s.Stroke = null;
                s.Opacity = 0.6;
                s.Fill = GetCurrentColor();
                Canvas.SetTop(s, canvasModel.Shapes[s].Y);
                Canvas.SetLeft(s, canvasModel.Shapes[s].X);
                canvas.Children.Add(s);
            }
        }
        
        public void DrawTempShape(Shape s, Point pos)
        {
            if (tempShape != null)canvas.Children.Remove(tempShape);

            canvas.Children.Add(s);
            Canvas.SetTop(s, pos.Y);
            Canvas.SetLeft(s, pos.X);
            s.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
            s.MouseMove += canvas_MouseMove;
            s.MouseLeftButtonUp += canvas_MouseLeftButtonUp;
            tempShape = s;
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
            if(!dragging)
            {
                dragging = true;
                currentMode.StartDragAction(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            label_Coords.Content = "fired!";
            if (dragging)
            {
                dragging = false;
                currentMode.StopDragAction(e.GetPosition(canvas));
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(dragging)
            {
                currentMode.DragAction(e.GetPosition(canvas));
            }
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

        private void button_Oval_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new DrawEllipse(canvasModel, this);
        }

        private void button_Rectangle_Click(object sender, RoutedEventArgs e)
        {
            currentMode = new DrawRectangle(canvasModel, this);
        }
    }
}
