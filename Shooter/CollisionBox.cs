using System;

namespace Shooter
{
    public class CollisionBox
    {
        public readonly float Width;
        public readonly float Height;

        public CollisionBox(float width, float height)
        {
            CheckForPositive(width, nameof(width));
            CheckForPositive(height, nameof(height));
            Width = width;
            Height = height;
        }

        private static void CheckForPositive(float value, string name)
        {
            if(value <= 0)
                throw new ArgumentOutOfRangeException(name, $"{name} must be positive, got {value}");
        }
    }
}
