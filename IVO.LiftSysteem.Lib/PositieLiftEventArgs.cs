using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVO.LiftSysteem.Api
{
    public class PositieLiftEventArgs : EventArgs
    {
        public int Positie { get; set; }
        public int DoelVerdiep { get; set; }
        public string Deuren { get; set; }
        public string Richting { get; set; }
    }
}
