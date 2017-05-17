using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shooter.Gui
{
    class Star
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public readonly int Alpha;

        public Star(float x, float y, int alpha)
        {
            X = x;
            Y = y;
            Alpha = alpha;
        }

        public void Add(float x, float y)
        {
            X += x;
            Y += y;
        }
    }

    public class BackgroundDrawer
    {
        private readonly float width;
        private readonly float height;

        private const float StarsWidthInterval = 64;
        private const int MinTicksBeforeStarsAdding = 5;
        private const int MaxTicksBeforeStarsAdding = 15;

        private const float StarVelX = 0;
        private const float StarVelY = 15;

        private const float MaxStartYDelta = 50;

        private const float StarWidth = 4;
        private const float StarHeight = 4;

        private readonly List<Star> stars;

        private int tickCount;
        private int maxTickCount;

        private readonly int maxStarsAdded;

        private readonly Random rand;

        public BackgroundDrawer(float width, float height)
        {
            this.width = width;
            this.height = height;
            maxStarsAdded = (int) (width / StarsWidthInterval);
            stars = new List<Star>();
            rand = new Random();
            RefreshTickCount();
        }

        public void Tick()
        {
            tickCount++;
            foreach (var star in stars)
                star.Add(StarVelX, StarVelY);
            stars.RemoveAll(IsStarOutOfBounds);
            if (tickCount < maxTickCount) return;
            AddStars();
            RefreshTickCount();
        }

        private void RefreshTickCount()
        {
            tickCount = 0;
            maxTickCount = rand.Next(MinTicksBeforeStarsAdding, MaxTicksBeforeStarsAdding + 1);
        }

        private void AddStars()
        {
            var starsAmount = rand.Next(maxStarsAdded + 1);
            var startX = 0f;
            var interval = width / starsAmount;
            for (var i = 0; i < starsAmount; i++)
            {
                stars.Add(new Star(
                    (float) (startX + rand.NextDouble() * interval),
                    (float) (rand.NextDouble() * MaxStartYDelta),
                    rand.Next(256)));
                startX += interval;
            }
        }

        public void DrawBackground(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, 0, 0, width, height);
            foreach (var star in stars)
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(star.Alpha,Color.White)),
                    star.X, star.Y, StarWidth, StarHeight);
        }

        private bool IsStarOutOfBounds(Star star)
        {
            return star.X < 0 ||
                   star.Y < 0 ||
                   star.X + StarWidth > width ||
                   star.Y + StarHeight > height;
        }
    }
}