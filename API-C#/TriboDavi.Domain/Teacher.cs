using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class Teacher : User
    {
        public required string Graduation { get; set; } // Analisar
        public required string RG { get; set; }
        public required string CPF { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
