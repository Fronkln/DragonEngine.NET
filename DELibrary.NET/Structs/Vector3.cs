using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Vector3(0,0,0,)
        /// </summary>
        public static Vector3 zero
        {
            get
            {
                return new Vector3();
            }
        }

        /// <summary>
        /// Vector4(1,1,1,1)
        /// </summary>
        public static Vector3 one
        {
            get
            {
                return new Vector3(1, 1, 1);
            }
        }

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector3 up
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector3 forward
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }

        /// <summary>
        /// Right direction.
        /// </summary>
        public static Vector3 right
        {
            get
            {
                return new Vector3(-1, 0, 0);
            }
        }

        public float magnitude
        {
            get 
            { 
                return (float)Math.Sqrt(x * x + y * y + z * z); 
            }
        }

        public Vector3 normalized
        {
            get { return Normalize(this); }
        }

        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")})";
        }

        public static explicit operator Vector3(Vector4 vec4)
        {
            return new Vector3(vec4.x, vec4.y, vec4.z);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
            );
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x + b.x;
            outVec.y = a.y + b.y;
            outVec.z = a.z + b.z;

            return outVec;
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x - b.x;
            outVec.y = a.y - b.y;
            outVec.z = a.z - b.z;

            return outVec;
        }

        public static Vector3 operator -(Vector3 a)
        {
            Vector3 outVec;

            outVec.x = -a.x;
            outVec.y = -a.y;
            outVec.z = -a.z;

            return outVec;
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x * b.x;
            outVec.y = a.y * b.y;
            outVec.z = a.z * b.z;

            return outVec;
        }

        public static Vector3 operator *(Vector3 a, float f)
        {
            Vector3 outVec;

            outVec.x = a.x * f;
            outVec.y = a.y * f;
            outVec.z = a.z * f;

            return outVec;
        }

        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return (v1.x == v2.x && v1.y == v2.y  && v1.z == v2.z);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;
            float diff_z = a.z - b.z;

            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
        }

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static Vector3 Normalize(Vector3 vec)
        {
            float mag = vec.magnitude;

            if (mag > 0.00001f)
                return vec / mag;
            else
                return zero;
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.y * vector2.z - vector1.z * vector2.y, vector1.z * vector2.x - vector1.x * vector2.z, vector1.x * vector2.y - vector1.y * vector2.x);
        }

        public static Vector3 Transform(Vector3 value, Quaternion rotation)
        {
            float num = rotation.x + rotation.x;
            float num2 = rotation.y + rotation.y;
            float num3 = rotation.z + rotation.z;
            float num4 = rotation.w * num;
            float num5 = rotation.w * num2;
            float num6 = rotation.w * num3;
            float num7 = rotation.x * num;
            float num8 = rotation.x * num2;
            float num9 = rotation.x * num3;
            float num10 = rotation.y * num2;
            float num11 = rotation.y * num3;
            float num12 = rotation.z * num3;
            return new Vector3(value.x * (1f - num10 - num12) + value.y * (num8 - num6) + value.z * (num9 + num5), value.x * (num8 + num6) + value.y * (1f - num7 - num12) + value.z * (num11 - num4), value.x * (num9 - num5) + value.y * (num11 + num4) + value.z * (1f - num7 - num10));
        }

        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }

        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3 (Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        // Returns the squared length (much faster; avoids the square root)
        public float LengthSquared()
        {
            return x * x + y * y + z * z;
        }
    }
}
