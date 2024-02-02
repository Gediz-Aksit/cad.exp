using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDB_CAD.Models
{
    public class CadApiModel
    {
        public CadApiSignatureModel Signature { get; set;  }
        public int Count { get; set; }
        public List<string> Fields { get; set; }
        public List<List<string>> Data { get; set; }
    }
}
