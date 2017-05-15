using System;

namespace Shooter
{
    public class CollisionBox
    {
        public readonly float Width;
        public readonly float Height;
        private readonly ICoordinatesProvider coordinatesProvider;

        public float Top => coordinatesProvider.Y - Height / 2;
        public float Bottom => coordinatesProvider.Y + Height / 2;
        public float Left => coordinatesProvider.X - Width / 2;
        public float Right => coordinatesProvider.X + Width / 2;

        public CollisionBox(ICoordinatesProvider coords, float width, float height)
        {
            CheckForPositive(width, nameof(width));
            CheckForPositive(height, nameof(height));
            Width = width;
            Height = height;
            coordinatesProvider = coords;
        }

        private static void CheckForPositive(float value, string name)
        {
            if(value <= 0)
                throw new ArgumentOutOfRangeException(name, $"{name} must be positive, got {value}");
        }


    }
}
