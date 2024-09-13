using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Core.Cafe
{
    public class CafeDetails
    {
        public int Id { get; set; }
        public string CafeName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; } // Change from string to byte[]
        public int FK_LocationId { get; set; }

        public  string LocationName { get; set; }

        public  int EmployeeCount { get; set; }

    }

}
