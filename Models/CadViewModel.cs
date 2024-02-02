using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDB_CAD.Models
{
    public class CadViewModel
    {
        public CadInputModel CadInput { get; set; }
        public List<CadModel> CadData { get; set; }
    }
}
