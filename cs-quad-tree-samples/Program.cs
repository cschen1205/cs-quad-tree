using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuadTreeLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            QuadTree world = new QuadTree(new Rectangle(0, 0, 2000, 2000));
            
            List<IAgent> population = new List<IAgent>();
            for(int i=0; i < 10000; ++i)
            {
                IAgent agent = new SimpleAgent(world);
                float y = 0; // vertical position
                float x = random.Next(2000);
                float z = random.Next(2000);
                agent.Position = new FVec3(x, y, z);
                population.Add(agent);
            }

            for(int steps = 0; steps < 20; ++steps)
            {
                List<IAgent> agentsWithinRectangle = world.Query(new Rectangle(200, 200, 800, 800));
                Thread.Sleep(100);

                Console.WriteLine("There are {0} agents in rectangel (200, 200, 800, 800) at step {1}",
                    agentsWithinRectangle.Count, steps + 1);

                foreach (IAgent agent in population)
                {
                    float y = 0; // vertical position
                    float x = random.Next(2000);
                    float z = random.Next(2000);
                    agent.Position = new FVec3(x, y, z);
                }
            }
            
        }
    }
}
