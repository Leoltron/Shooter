using System.Drawing;
using System.IO;

namespace Shooter.Gui
{
    static class PlayerDrawer
    {
        private static readonly Bitmap PlayerBaseBitmap;
        private static readonly Bitmap[] BoostersBitmaps;
        private static readonly Bitmap[] GunsBitmaps;

        static PlayerDrawer()
        {
            var imagesDirectory = new DirectoryInfo("Textures\\Entities\\Player\\");
            var playerImagesDirectoryFullName = imagesDirectory.FullName;
            PlayerBaseBitmap = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + "PlayerBase.png");
            BoostersBitmaps = new Bitmap[Player.MaxBoostersLevel];
            for (var i = 0; i < BoostersBitmaps.Length; i++)
                BoostersBitmaps[i] = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + $"Boosters_{i}.png");
            GunsBitmaps = new Bitmap[Player.MaxGunsAmountLevel+1];
            for (var i = 0; i < GunsBitmaps.Length; i++)
                GunsBitmaps[i] = (Bitmap) Image.FromFile(playerImagesDirectoryFullName + $"Guns\\Basic\\{i}.png");
        }

        public static void DrawPlayer(Graphics graphics, Entity entity, bool isDebugMode = false)
        {
            var player = entity as Player;
            if(player == null || player.InvincibilityTime % 2 != 0)
                return;
            graphics.TranslateTransform(player.X, player.Y);
            graphics.RotateTransform(player.Direction);
            var x = -PlayerBaseBitmap.Width / 2;
            var y = -PlayerBaseBitmap.Height / 2;
            graphics.DrawImage(PlayerBaseBitmap, x, y);
            graphics.DrawImage(BoostersBitmaps[player.BoostersLevel], x, y);
            graphics.DrawImage(GunsBitmaps[player.GunsAmountLevel], x, y);
            if (isDebugMode)
                DrawUtils.DrawCollisionBox(graphics, player);

            graphics.RotateTransform(-player.Direction);
            graphics.TranslateTransform(-player.X, -player.Y);
        }
    }
}