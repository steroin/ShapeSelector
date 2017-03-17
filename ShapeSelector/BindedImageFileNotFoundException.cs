using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeSelector
{
    class BindedImageFileNotFoundException : Exception
    {
        public string FileName { get; }
        public BindedImageFileNotFoundException(string fileName)
        {
            FileName = fileName;
        }
    }
}
