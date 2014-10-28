using System;

namespace SharpGL_project_1
{
    public class Quaternion
    {
        public Quaternion(double p1, double p2, double p3, double p4)
        {
            X = p1;
            Y = p2;
            Z = p3;
            W = p4;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double W { get; set; }

        public Quaternion Normalize()
        {
            double length = (double)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);

            X /= length;
            Y /= length;
            Z /= length;
            W /= length;

            return this;
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-X, -Y, -Z, W);
        }

        public static Quaternion operator *(Quaternion v1, Quaternion v2)
        {
            double w = (v1.W * v2.W) - (v1.X * v2.X) - (v1.Y * v2.Y) - (v1.Z * v2.Z);
            double x = (v1.X * v2.W) + (v1.W * v2.X) + (v1.Y * v2.Z) - (v1.Z * v2.Y);
            double y = (v1.Y * v2.W) + (v1.W * v2.Y) + (v1.Z * v2.X) - (v1.X * v2.Z);
            double z = (v1.Z * v2.W) + (v1.W * v2.Z) + (v1.X * v2.Y) - (v1.Y * v2.X);

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            double w = -(q.X * v.X) - (q.Y * v.Y) - (q.Z * v.Z);
            double x = (q.W * v.X) + (q.Y * v.Z) - (q.Z * v.Y);
            double y = (q.W * v.Y) + (q.Z * v.X) - (q.X * v.Z);
            double z = (q.W * v.Z) + (q.X * v.Y) - (q.Y * v.X);

            return new Quaternion(x, y, z, w);
        }
    }
}
