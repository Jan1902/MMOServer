using System;

namespace MMOServer.Other
{
    class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float Magnitude { get => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector operator *(Vector a, float b)
        {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector operator *(Vector a, int b)
        {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        public Vector(float posX, float posY, float posZ)
        {
            X = posX;
            Y = posY;
            Z = posZ;
        }

        public float DistanceTo(Vector other)
        {
            var a = other.X - X;
            var b = other.Y - Y;
            var c = other.Z - Z;

            return (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));
        }

        public void Normalize()
        {
            var mag = Magnitude;
            X /= mag;
            Y /= mag;
            Z /= mag;
        }

        public void MoveWithRotation(float rotationAngle, float length)
        {
            var direction = new Vector((float)Math.Cos(rotationAngle), 0, (float)Math.Sin(rotationAngle));
            direction.Normalize();
            direction *= length;
            X += direction.X;
            Y += direction.Y;
            Z += direction.Z;
        }

        public static Vector Zero()
        {
            return new Vector(0, 0, 0);
        }
    }
}
