using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShapeSelector
{
    interface IActionMode
    {
        void StartDragAction(Point p);
        void DragAction(Point p);
        void StopDragAction(Point p);
        void DoubleClickAction(Point p);
        void ExitCanvasAction();
        void ClickOutsideCanvasAction();
    }
}
