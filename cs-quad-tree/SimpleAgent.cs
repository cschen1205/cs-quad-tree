using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadTreeLib
{
    public class SimpleAgent : IAgent
    {
        public SimpleAgent(QuadTree world)
        {
            Space = world;
        }
    }
}
