using System;

namespace MMOServer.Other
{
    class Vector
    {
        /// <summary>
        /// The X Value
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y Value
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Z Value
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The Vectors Magnitude
        /// </summary>
        public float Magnitude { get => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }

        //Vector Addition
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        //Vector Subtraction
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        //Vector - Float Multiplication
        public static Vector operator *(Vector a, float b)
        {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        //Vector - Int Multiplication
        public static Vector operator *(Vector a, int b)
        {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Initializes a new Vector
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="posZ"></param>
        public Vector(float posX, float posY, float posZ)
        {
            X = posX;
            Y = posY;
            Z = posZ;
        }

        /// <summary>
        /// Calculates the distance to another Vector
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float DistanceTo(Vector other)
        {
            var a = other.X - X;
            var b = other.Y - Y;
            var c = other.Z - Z;

            return (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public void Normalize()
        {
            var mag = Magnitude;
            X /= mag;
            Y /= mag;
            Z /= mag;
        }

        /// <summary>
        /// "Moves" the vector by a given length into a given direction
        /// </summary>
        /// <param name="rotationAngle"></param>
        /// <param name="length"></param>
        public void MoveWithRotation(float rotationAngle, float length)
        {
            var direction = new Vector((float)Math.Cos(rotationAngle), 0, (float)Math.Sin(rotationAngle));
            direction.Normalize();
            direction *= length;
            X += direction.X;
            Y += direction.Y;
            Z += direction.Z;
        }

        /// <summary>
        /// Returns a new Vector with X, Y and Z at 0
        /// </summary>
        /// <returns></returns>
        public static Vector Zero()
        {
            return new Vector(0, 0, 0);
        }
    }
}
