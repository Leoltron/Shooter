using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shooter
{
    class StarPosition
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public StarPosition(float x, float y)
        {
            X = x;
            Y = y;
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

        private readonly List<StarPosition> stars;

        private int tickCount;
        private int maxTickCount;

        private readonly int maxStarsAdded;

        private readonly Random rand;

        public BackgroundDrawer(float width, float height)
        {
            this.width = width;
            this.height = height;
            maxStarsAdded = (int) (width / StarsWidthInterval);
            stars = new List<StarPosition>();
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
                stars.Add(new StarPosition(
                    (float) (startX + rand.NextDouble() * interval),
                    (float) (rand.NextDouble() * MaxStartYDelta)));
                startX += interval;
            }
        }

        public void DrawBackground(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, 0, 0, width, height);
            foreach (var starPosition in stars)
                graphics.FillRectangle(Brushes.White, starPosition.X, starPosition.Y, StarWidth, StarHeight);
        }

        private bool IsStarOutOfBounds(StarPosition starPosition)
        {
            return starPosition.X < 0 ||
                   starPosition.Y < 0 ||
                   starPosition.X + StarWidth > width ||
                   starPosition.Y + StarHeight > height;
        }
    }
}