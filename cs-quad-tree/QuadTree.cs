using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuadTreeLib
{
    public class QuadTree
    {
        QuadTreeNode m_root;
        List<IAgent> outOfBoundList = new List<IAgent>();
        /// <summary>
        /// Bound of this QuadTree
        /// </summary>
        Rectangle m_rectangle;

        public QuadTree(Rectangle bound)
        {
            m_rectangle = bound;
            m_root = new QuadTreeNode(m_rectangle);
        }

        public List<IAgent> OutOfBoundList()
        {
            return outOfBoundList;
        }

        /// <summary>
        /// Count the number of items in the QuadTree
        /// </summary>
        public int Count { get { return m_root.Count; } }

        public void Insert(IAgent item)
        {
            if (m_root.Bounds.Contains(item.Position))
            {
                m_root.Insert(item);
            }
            else
            {
                outOfBoundList.Add(item);
                item.Node = null;
            }
        }
        
        public List<IAgent> Query(Rectangle area)
        {
//            return m_root.Query(area);
            List<IAgent> results = m_root.Query(area);
            results.AddRange(outOfBoundList);
            return results;
        }

        public List<IAgent> QueryRadius(Rectangle area, float radius)
        {
            //return m_root.QueryRadius(area, radius);
            List<IAgent> results = m_root.QueryRadius(area, radius);
            results.AddRange(outOfBoundList);
            return results;
        }
    }
}
