using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Shooter.Gui
{
    static class BagelDrawer
    {
        private const int DamagedBagelBitmapsAmount = 4;
        private static readonly Bitmap[] DamagedBagelBitmaps;
        private static readonly Dictionary<BagelType, Bitmap> BagelBitmaps;

        static BagelDrawer()
        {
            DamagedBagelBitmaps = new Bitmap[DamagedBagelBitmapsAmount];
            var bagelBitmapsDir = new DirectoryInfo("Textures\\Entities\\Bagel\\");
            for (var i = 1; i < DamagedBagelBitmapsAmount; i++)
                DamagedBagelBitmaps[i] = (Bitmap) Image.FromFile($"{bagelBitmapsDir.FullName}{i}.png");
            BagelBitmaps = new Dictionary<BagelType, Bitmap>();
            foreach (var bagelType in Enum.GetValues(typeof(BagelType)).Cast<BagelType>())
                BagelBitmaps[bagelType] = (Bitmap) Image.FromFile($"{bagelBitmapsDir.FullName}{bagelType}Core.png");
        }

        public static void DrawBagel(Graphics graphics, Entity entity, bool isDebugMode = false)
        {
            var bagel = entity as BagelEnemy;
            if (bagel == null)
                return;
            graphics.TranslateTransform(bagel.X, bagel.Y);
            graphics.RotateTransform(bagel.Direction);

            if (bagel.Health > 1)
            {
                DrawUtils.DrawCenteredBitmap(graphics,
                    bagel.Health - 1 < DamagedBagelBitmapsAmount
                        ? DamagedBagelBitmaps[bagel.Health - 1]
                        : DamagedBagelBitmaps[DamagedBagelBitmaps.Length - 1]);
            }
            DrawUtils.DrawCenteredBitmap(graphics, BagelBitmaps[bagel.BagelType]);
            if (isDebugMode)
                DrawUtils.DrawCollisionBox(graphics, bagel);

            graphics.RotateTransform(-bagel.Direction);
            graphics.TranslateTransform(-bagel.X, -bagel.Y);
        }
    }
}