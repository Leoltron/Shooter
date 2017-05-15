using System.Drawing;
using System.IO;

namespace Shooter
{
    class PlayerDrawer
    {
        private readonly Bitmap playerBaseBitmap;
        private readonly Bitmap[] boostersBitmaps;
        private readonly Bitmap[] gunsBitmaps;

        public PlayerDrawer()
        {
            var imagesDirectory = new DirectoryInfo("Textures\\Entities\\Player");
            var playerImagesDirectoryFullName = imagesDirectory.FullName;
            playerBaseBitmap = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + "\\PlayerBase.png");
            boostersBitmaps = new Bitmap[Player.MaxBoostersLevel];
            for (var i = 0; i < boostersBitmaps.Length; i++)
                boostersBitmaps[i] = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + $"\\Boosters_{i}.png");
            gunsBitmaps = new Bitmap[Player.MaxGunsAmountLevel];
            for (var i = 0; i < gunsBitmaps.Length; i++)
                gunsBitmaps[i] = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + $"\\Guns\\Basic\\{i}.png");
        }

        public void DrawPlayer(Player player, Graphics graphics, bool isDebugMode = false)
        {
            graphics.TranslateTransform(player.X, player.Y);
            graphics.RotateTransform(player.Direction);
            var x = -playerBaseBitmap.Width / 2;
            var y = -playerBaseBitmap.Height / 2;
            graphics.DrawImage(playerBaseBitmap, x, y);
            graphics.DrawImage(boostersBitmaps[player.BoostersLevel], x, y);
            graphics.DrawImage(gunsBitmaps[player.GunsAmountLevel], x, y);
            if (isDebugMode)
                EntityDrawer.DrawCollisionBox(graphics, player);

            graphics.RotateTransform(-player.Direction);
            graphics.TranslateTransform(-player.X, -player.Y);
        }
    }
}