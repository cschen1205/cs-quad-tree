using System;
using System.Runtime.InteropServices;

namespace QuadTreeLib
{
    [Guid("C66BD0A5-3B58-490B-A89C-38131547CB9C")]
    public interface IFVec3
    {

        float Length { get; }
        float SquaredLength {  get; }
        float Normalise();
        FVec3 GetNormal();
        void Assign(FVec3 vect3);
        void Assign(float x, float y, float z);
        void Assign(FVec2 vect2);
        float GetDistanceTo(FVec3 pt);
        float GetDistanceSqTo(FVec3 pt);
        FVec3 Clone();
        FVec2 ToFVec2();
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("0EF15D93-1DBB-4564-B31F-4C28DA0585E7")]
    public class FVec3 : IFVec3
    {
        public float x, y, z;
        public static FVec3 ZERO
        {
            get
            {
                return new FVec3(0, 0, 0);
            }
        }
        public static FVec3 FORWARD
        {
            get
            {
                return new FVec3(0, 0, 1);
            }
        }
        public static FVec3 UP
        {
            get
            {
                return new FVec3(0, 1, 0);
            }
        }
        public static FVec3 RANDOM
        {
            get
            {
                Random rand = new Random();
                FVec3 vec = new FVec3((float)rand.NextDouble(), 0, (float)rand.NextDouble());
                vec.Normalise();
                return vec;
            }
        }


        public FVec3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public FVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FVec3(FVec3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
        public static bool IsNan(FVec3 value){
            return float.IsNaN(value.x) ||float.IsNaN(value.y) || float.IsNaN(value.z);
        }
        public float Length
        {
            get
            {
                double result = System.Math.Sqrt((x * x + y * y + z * z));
                return (float)(result);
            }
        }

        public float SquaredLength
        {
            get
            {
                return (float)(x * x + y * y + z * z);
            }
        }

        public float Normalise()
        {
            float theLength = this.Length;
            if (theLength == 0)
            {
                return theLength;
            }
            x /= theLength;
            y /= theLength;
            z /= theLength;
            return theLength;
        }

        public FVec3 GetNormal()
        {
            float theLength = this.Length;
            return new FVec3(x / theLength, y / theLength, z / theLength);
        }


        public static FVec3 operator +(FVec3 v1, FVec3 v2)
        {
            return new FVec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static FVec3 operator -(FVec3 v1, FVec3 v2)
        {
            return new FVec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static FVec3 operator -(FVec3 v1)
        {
            return new FVec3(v1.x * -1, v1.y * -1, v1.z * -1);
        }


        public static FVec3 operator *(FVec3 v1, float v2)
        {
            return new FVec3(v1.x * v2, v1.y * v2, v1.z * v2);
        }

        public static FVec3 operator *(float v2, FVec3 v1)
        {
            return new FVec3(v1.x * v2, v1.y * v2, v1.z * v2);
        }

        public static FVec3 operator /(FVec3 v1, float v2)
        {
            return new FVec3(v1.x / v2, v1.y / v2, v1.z / v2);
        }

        public void Plus(FVec3 v1)
        {
            this.x += v1.x;
            this.y += v1.y;
            this.z += v1.z;
        }
        public void Minus(FVec3 v1)
        {
            this.x -= v1.x;
            this.y -= v1.y;
            this.z -= v1.z;
        }
        public void Multiply(float scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
        }
        public void Divide(float scalar)
        {
            if (scalar == 0) throw new DivideByZeroException("cannot divide by zero");
            this.x /= scalar;
            this.y /= scalar;
            this.z /= scalar;
        }

        public float DotProduct(FVec3 v2)
        {
            return (x * v2.x + y * v2.y + z * v2.z);
        }

        public static float DotProduct(FVec3 v1, FVec3 v2){
            return (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
        }
        public static FVec3 CrossProduct(FVec3 v1, FVec3 v2)
        {
            return new FVec3(v1.y * v2.z - v1.z * v2.y,
                               v1.z * v2.x - v1.x * v2.z,
                               v1.x * v2.y - v1.y * v2.x);
        }

        public void Assign(FVec3 vect3)
        {
            this.x = vect3.x;
            this.y = vect3.y;
            this.z = vect3.z;            
        }
        public void Assign(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Assign(FVec2 vect2)
        {
            this.x = vect2.x;
            this.z = vect2.z;
        }
        public float GetDistanceTo(FVec3 pt)
        {
            float dx = pt.x - x;
            float dy = pt.y - y;
            float dz = pt.z - z;
            return (float)System.Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // Override the ToString method so the value appears in text
        public override string ToString()
        {
            return String.Format("({0},{1},{2})", x, y, z);
        }

        // Spherical linear interpolation
        public static FVec3 RotateTowards(FVec3 from, FVec3 to, float maxThetaRad)
        {
            // Normalize the vectors

            FVec3 unitfrom = from.GetNormal(),
                    unitto = to.GetNormal();

            // Calculate the included angle
            //if (FVec3.IsNan(unitfrom)) //UnityEngine.Debug.LogError("From is NULL");
            //if (FVec3.IsNan(to)) //UnityEngine.Debug.LogError("To is NULL");

            double theta =
              Math.Acos(unitfrom.DotProduct(unitto));

            if (theta < 0.05 || Double.IsNaN(theta))
                return to;
            // Avoid the repeated sine calculation

            double st =
              Math.Sin(theta);
            if (st == 0 || st < 0.01)
            {
                return to;
            }
            // Return the geometric spherical linear interpolation
            float alpha = (float)Math.Min(theta, maxThetaRad);
            
            return
              from * (float)(Math.Sin(theta - alpha) / st) +
              to * (float)Math.Sin(alpha) / (float)st;
        }

        // Spherical linear interpolation
        public static FVec3 Slerp(FVec3 from, FVec3 to, float step)
        {
            if (step == 0)
                return from;

            if (from == to || step == 1)
                return to;

            // Normalize the vectors

            FVec3 unitfrom = from.GetNormal(),
                    unitto = to.GetNormal();

            // Calculate the included angle

            double theta =
              Math.Acos(unitfrom.DotProduct(unitto));
        
            if (theta < 0.1 || Double.IsNaN(theta))
                return to;

            // Avoid the repeated sine calculation

            double st =
              Math.Sin(theta);
            if (st == 0 || st < 0.01)
            {
                return to;
            }
            // Return the geometric spherical linear interpolation

            return
              from * (float)(Math.Sin((1 - step) * theta) / st) +
              to * (float)Math.Sin(step * theta) / (float)st;
        }

        public static FVec3 Truncate(FVec3 vec, float maxValue)
        {
            float val = vec.Length;
            if (val > maxValue)
            {
                vec.Normalise();
                return vec * maxValue;
            }
            else
            {
                return vec;
            }
        }

        public static FVec3 RotateAround_Y(FVec3 A, float radian)
        {
            double cosine = System.Math.Cos(radian);
            double sine = System.Math.Sin(radian);
            FVec3 result = new FVec3();
            result.x = (float)(A.x * cosine - A.z * sine);
            result.y = A.y;
            result.z = (float)(A.x * sine + A.z * cosine);
            return result;
        }

        public float GetDistanceSqTo(FVec3 pt)
        {
            float dx = pt.x - x;
            float dy = pt.y - y;
            float dz = pt.z - z;
            return dx * dx + dy * dy + dz * dz;
        }

        public FVec3 Clone()
        {
            return new FVec3(x, y, z);
        }

        public static double AngleBetween(FVec3 A, FVec3 B)
        {
            FVec3 tempA = A.Clone();
            FVec3 tempB = B.Clone();
            tempA.Normalise();
            tempB.Normalise();
            return Math.Acos(FVec3.DotProduct(tempA, tempB));
        }

        public static float MahattanDist(FVec3 A, FVec3 B)
        {
            return Math.Abs(A.x - B.x) + Math.Abs(A.y - B.y) + Math.Abs(A.z - B.z);
        }
        public static float SQDistance(FVec3 A, FVec3 B)
        {
            return A.GetDistanceSqTo(B);
        }
        public static float Distance(FVec3 A, FVec3 B)
        {
            return A.GetDistanceTo(B);
        }
        public FVec2 ToFVec2()
        {
            return new FVec2(x, z);
        }
    }
}