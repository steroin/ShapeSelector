using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeSelector
{
    class TableRow
    {
        public int Id { get; }
        public string Name { get; }
        public string Details { get; }

        public TableRow(int id, string name, string details)
        {
            Id = id;
            Name = name;
            Details = details;
        }
    }
}
