using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriboDavi.Domain
{
    public class Graduation : BaseEntity
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required int Position { get; set; }
    }
}
