using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDB_CAD.Models
{
    public class CadInputModel
    {
        public string Body { get; set; }
        public string ObjectType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(60);
        public double MinDistance { get; set; } = 0.0;
        public double MaxDistance { get; set; } = 0.05;
        public double h { get; set; }
        public string fullname { get; set; }
    }
}
