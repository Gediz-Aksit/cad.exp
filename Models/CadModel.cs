using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDB_CAD.Models
{
    public class CadModel
    {
        public string des { get; set; }
        public int orbit_id { get; set; }
        public double jd { get; set; }
        public DateTime cd { get; set; }
        public double dist { get; set; }
        public double dist_min { get; set; }
        public double dist_max { get; set; }
        public double v_rel { get; set; }
        public string v_inf { get; set; }
        public string t_sigma_f { get; set; }
        public double h { get; set; }
        public string fullname { get; set; }
    }
}
