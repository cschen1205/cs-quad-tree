using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuadTreeLib
{
    public class Rectangle
    {
        /// <summary>
        /// x1 = bottom left x coordinate
        /// y1 = bottom left y coordinate
        /// x2 = top right x coordinate
        /// y2 = top right y coordinate
        /// </summary>
        public double x1, y1, x2, y2;
        public double Width { get { return x2 - x1; } }
        public double Height { get { return y2 - y1; } }

        public double Top { get { return y2; } }
        public double Bottom { get { return y1; } }
        public double Left { get { return x1; } }
        public double Right { get { return x2; } }

        public Rectangle(Rectangle r)
        {
            x1 = r.x1; x2 = r.x2; y1 = r.y1; y2 = r.y2;
        }

        public Rectangle(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1; this.y1 = y1; this.x2 = x2; this.y2 = y2;
        }

        public FVec2 Center
        {
            get
            {
                return new FVec2((float)(Left + Right) / 2, (float)(Top + Bottom) / 2);
            }
        }

        public bool IntersectsWith(Rectangle r)
        {
            return Contains(r.x1, r.y1) || Contains(r.x1, r.y2) || Contains(r.x2, r.y1) ||
                Contains(r.x2, r.y2);
        }

        public bool Contains(Rectangle r)
        {
            //if (r.Left >= Right) return false;
            //if (r.Right < Left) return false;
            //if (r.Top < Bottom) return false;
            //if (r.Bottom >= Top) return false;
            //return true;
            if (!Contains(r.x1, r.y1)) return false;
            if (!Contains(r.x2, r.y1)) return false;
            if (!Contains(r.x2, r.y2)) return false;
            if (!Contains(r.x1, r.y2)) return false;
            return true;
            //return (Contains(r.x1, r.y1) &&
            //        Contains(r.x2, r.y1) &&
            //        Contains(r.x2, r.y2) &&
            //        Contains(r.x1, r.y2));
        }
        public bool Contains(double x, double y)
        {
            if (x < Left) return false;
            if (x >= Right) return false;
            if (y >= Top) return false;
            if (y < Bottom) return false;
            return true;
            //            return ((x >= Left) && (x < Right) && (y < Top) && (y >= Bottom));
        }

        // Contain, but making the rectangle bigger in refence to the errorTolerance variable
        public bool Contains(double x, double y, double errorToleranceX, double errorToleranceY)
        {
            if (x < Left - errorToleranceX) return false;
            if (x >= Right + errorToleranceX) return false;
            if (y >= Top + errorToleranceY) return false;
            if (y < Bottom - errorToleranceY) return false;
            return true;
        }

        public bool Contains(FVec3 pt)
        {
            return Contains(pt.x, pt.z);
        }

        public double Area { get { return Height * Width; } }
        /*
        public bool Contains(FVec3 pt)
        {
            if (pt.x < x1) return false;
            if (pt.x > x2) return false;
            if (pt.y < y1) return false;
            if (pt.y > y2) return false;
            return true;
        }*/

        public static Rectangle computeBound(params FVec3[] pt)
        {
            float minX = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxZ = float.MinValue;
            foreach (FVec3 p in pt)
            {
                if (p.x < minX) minX = p.x;
                if (p.x > maxX) maxX = p.x;
                if (p.z < minZ) minZ = p.z;
                if (p.z > maxZ) maxZ = p.z;
            }
            return new Rectangle(minX, minZ, maxX, maxZ);
        }

        public void Translate(double dx, double dy)
        {
            x1 += dx;
            x2 += dx;
            y1 += dy;
            y2 += dy;
        }

        public void MoveAbs(float centerX, float centerY)
        {
            x1 = centerX - Width / 2;
            x2 = centerX + Width / 2;
            y1 = centerY - Height / 2;
            y2 = centerY + Height / 2;
        }

        public void MoveAbs(FVec2 center)
        {
            x1 = center.x - Width / 2;
            x2 = center.x + Width / 2;
            y1 = center.z - Height / 2;
            y2 = center.z + Height / 2;
        }

        public void Expand(ref double point, double adjustment)
        {
            point += adjustment;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", x1, y1, x2, y2);
        }
    }

}
