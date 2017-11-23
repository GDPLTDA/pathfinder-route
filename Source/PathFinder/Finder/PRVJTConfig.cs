using PathFinder.Routes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder
{
    public class PRVJTConfig
    {
        public RouteMap Map { get; set; }
        public int NumEntregadores { get; set; }
        public DateTime DtLimite { get; set; } = DateTime.Now.AddDays(4);
    }
}
