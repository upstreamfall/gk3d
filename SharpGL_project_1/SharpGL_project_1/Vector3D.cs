
using System;

namespace SharpGL_project_1
{
    public class Vector3D
    {
        public Vector3D(float p1, float p2, float p3)
        {
            X = p1;
            Y = p2;
            Z = p3;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vector3D Cross(Vector3D v)
        {
            float _x = Y * v.Z - Z * v.Y;
            float _y = Z * v.X - X * v.Z;
            float _z = X * v.Y - Y * v.X;

            return new Vector3D(_x, _y, _z);
        }

        public Vector3D Normalize()
        {
            float length = (float) Math.Sqrt(X * X + Y * Y + Z * Z);

            X /= length;
            Y /= length;
            Z /= length;

            return this;
        }
    }
}
