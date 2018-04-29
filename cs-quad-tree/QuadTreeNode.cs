using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;

namespace QuadTreeLib
{
    public class QuadTreeNode
    {
        public Rectangle m_bound;
        public List<IAgent> contents = new List<IAgent>();
        public List<QuadTreeNode> subNodes = new List<QuadTreeNode>();
        public static int Index = 0;
        public int thisIndex = -1;
        public FVec3 force = FVec3.ZERO;

        public QuadTreeNode(double x1, double y1, double x2, double y2)
        {
            m_bound = new Rectangle(x1, y1, x2, y2);
            Index += 1;
            thisIndex = Index;
        }

        public QuadTreeNode(Rectangle rect)
        {
            m_bound = new Rectangle(rect);
            Index += 1;
            thisIndex = Index;
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (QuadTreeNode node in subNodes)
                {
                    count += node.Count;
                }
                count += this.contents.Count;
                return count;
            }
        }

        public Rectangle Bounds { get { return m_bound; } }

        public void CreateSubNodes()
        {
            //the smallest subnode has an area
            if ((m_bound.Area) <= 10000)
            {
                return;
            }

            double xNew = m_bound.Width / 2 + m_bound.x1;
            double yNew = m_bound.Height / 2 + m_bound.y1;

            subNodes.Add(new QuadTreeNode(m_bound.x1, m_bound.y1, xNew, yNew));
            subNodes.Add(new QuadTreeNode(xNew, m_bound.y1, m_bound.x2, yNew));
            subNodes.Add(new QuadTreeNode(xNew, yNew, m_bound.x2, m_bound.y2));
            subNodes.Add(new QuadTreeNode(m_bound.x1, yNew, xNew, m_bound.y2));
        }

        public void Insert(IAgent item)
        {
            if(!m_bound.Contains(item.Position)){
                return;
            }
            if (subNodes.Count == 0)
            {
                CreateSubNodes();
            }
            
            foreach(QuadTreeNode node in subNodes){
                if(node.Bounds.Contains(item.Position)){
                    node.Insert(item);
                    return;
                }
            }
            contents.Add(item);
            item.Node = this;
        }

        public bool IsEmpty { get { return subNodes.Count == 0; } }
        public List<IAgent> SubTreeContents
        {
            get
            {
                List<IAgent> results = new List<IAgent>();
                foreach (QuadTreeNode node in subNodes)
                {
                    results.AddRange(node.SubTreeContents);
                }
                results.AddRange(this.contents);
                return results;
            }
        }

        public List<IAgent> Query(Rectangle queryArea)
        {
            List<IAgent> results = new List<IAgent>();
            foreach (IAgent item in this.contents)
            {
                if (queryArea.Contains(item.Position))
                {
                    results.Add(item);
                }
            }

            foreach (QuadTreeNode node in subNodes){
                //if(node.IsEmpty){
                //    Fame.Fame.Singleton.UnityPrint(String.Format("skipping:{0} ", node.thisIndex));
                //    continue;
                //}
                //case 1: search area completely contained by sub-quad
                //if a node completely contains the query area, go down that branch
                //and skip the remaining node (break this loop)
                if (node.Bounds.Contains(queryArea))
                {
                    results.AddRange(node.Query(queryArea));
                    break;
                }
                //case 2: sub-quad completely contained by search area
                //if the query area completely contains a sub-quad,
                //just add all the contents of that quad and its children
                //to the result set. You need to continue the loop to test
                //the other quads
                if(queryArea.Contains(node.Bounds)){
                    results.AddRange(node.SubTreeContents);
                    continue;
                }
                //case 3: search area intersects with sub-quad
                //traverse into this quad, continue the loop to
                //search other quads
                if(node.Bounds.IntersectsWith(queryArea)){
                    results.AddRange(node.Query(queryArea));
                }
            }
            
            return results;
        }

        public List<IAgent> QueryRadius(Rectangle queryArea, float radius)
        {
            List<IAgent> results = new List<IAgent>();
            //Fame.Fame.Singleton.UnityPrint(String.Format("{0} {1}", thisIndex,contents.Count));            

            //Fame.Fame.Singleton.UnityPrint(String.Format("Checking:{0} ", thisIndex));
            //List<IAgent> cloneContents;
            //lock (thisLock)
            //{
            //    cloneContents = new List<IAgent>(contents);
            //}
            FVec2 rectCenter = queryArea.Center;
            float radiusSq = radius * radius;
            foreach (IAgent item in this.contents)
            {
                FVec2 agentPos = new FVec2(rectCenter.x - item.Position.x, rectCenter.z - item.Position.z);
                if (agentPos.SquaredLength < radiusSq)
                {
                    results.Add(item);
                }
            }

            foreach (QuadTreeNode node in subNodes)
            {
                //if(node.IsEmpty){
                //    Fame.Fame.Singleton.UnityPrint(String.Format("skipping:{0} ", node.thisIndex));
                //    continue;
                //}
                //case 1: search area completely contained by sub-quad
                //if a node completely contains the query area, go down that branch
                //and skip the remaining node (break this loop)
                if (node.Bounds.Contains(queryArea))
                {
                    results.AddRange(node.QueryRadius(queryArea,radius));
                    break;
                }

                //case 2: sub-quad completely contained by search area
                //if the query area completely contains a sub-quad,
                //just add all the contents of that quad and its children
                //to the result set. You need to continue the loop to test
                //the other quads
                if (queryArea.Contains(node.Bounds))
                {
                    results.AddRange(node.SubTreeContents);
                    continue;
                }

                //case 3: search area intersects with sub-quad
                //traverse into this quad, continue the loop to
                //search other quads
                if (node.Bounds.IntersectsWith(queryArea))
                {
                    results.AddRange(node.QueryRadius(queryArea,radius));
                }
            }

            return results;
        }
    }
}
