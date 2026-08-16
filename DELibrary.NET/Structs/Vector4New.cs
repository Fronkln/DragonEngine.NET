using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace DragonEngineLibrary
{
    //Unused for now
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    internal struct Vector4New 
    {
        private Vector128<float> _value;

        public float x
        {
            readonly get => _value.GetElement(0);
            set => _value = _value.WithElement(0, value);
        }

        public float y
        {
            readonly get => _value.GetElement(1);
            set => _value = _value.WithElement(1, value);
        }

        public float z
        {
            readonly get => _value.GetElement(2);
            set => _value = _value.WithElement(2, value);
        }

        public float w
        {
            readonly get => _value.GetElement(3);
            set => _value = _value.WithElement(3, value);
        }

        public Vector4New(float x, float y, float z)
        {
            _value = Vector128.Create(x, y, z, 0.0f);
        }

        public Vector4New(float x, float y, float z, float w)
        {
            _value = Vector128.Create(x, y, z, w);
        }

        private Vector4New(Vector128<float> value)
        {
            _value = value;
        }

        /// <summary>
        /// Vector4New(0,0,0,0)
        /// </summary>
        public static Vector4New zero => new(Vector128<float>.Zero);

        /// <summary>
        /// Vector4New(1,1,1,1)
        /// </summary>
        public static Vector4New one => new(Vector128.Create(1.0f));

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector4New up => new(0, 1, 0);

        public override string ToString()
        {
            return $"({x:0.00} {y:0.00} {z:0.00} {w:0.00})";
        }

        public static implicit operator Vector4New(Vector3 vec3)
        {
            return new Vector4New(vec3.x, vec3.y, vec3.z);
        }

        public static Vector4New Lerp(Vector4New a, Vector4New b, float t)
        {
            return new Vector4New(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t,
                a.w + (b.w - a.w) * t
            );
        }

        public static bool operator !=(Vector4New a, Vector4New b)
        {
            return a.x != b.x ||
                   a.y != b.y ||
                   a.z != b.z ||
                   a.w != b.w;
        }

        public static bool operator ==(Vector4New a, Vector4New b)
        {
            return a.x == b.x &&
                   a.y == b.y &&
                   a.z == b.z &&
                   a.w == b.w;
        }

        public static Vector4New operator +(Vector4New a, Vector4New b)
        {
            return new Vector4New(
                a.x + b.x,
                a.y + b.y,
                a.z + b.z,
                a.w + b.w
            );
        }

        public static Vector4New operator -(Vector4New a)
        {
            return new Vector4New(
                -a.x,
                -a.y,
                -a.z,
                -a.w
            );
        }

        public static Vector4New operator -(Vector4New a, Vector4New b)
        {
            return new Vector4New(
                a.x - b.x,
                a.y - b.y,
                a.z - b.z,
                a.w - b.w
            );
        }

        public static Vector4New operator *(Vector4New a, Vector4New b)
        {
            return new Vector4New(
                a.x * b.x,
                a.y * b.y,
                a.z * b.z,
                a.w * b.w
            );
        }

        public static Vector4New operator *(Vector4New a, float f)
        {
            return new Vector4New(
                a.x * f,
                a.y * f,
                a.z * f,
                a.w * f
            );
        }

        public static float Distance(Vector4New a, Vector4New b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            float dw = a.w - b.w;

            return MathF.Sqrt(
                dx * dx +
                dy * dy +
                dz * dz +
                dw * dw
            );
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector4New other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }
    }
}