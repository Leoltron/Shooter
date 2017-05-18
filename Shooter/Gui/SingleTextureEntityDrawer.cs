using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Shooter.Gui
{
    public static class SingleTextureEntityDrawer
    {
        private static readonly Dictionary<string, Bitmap> Bitmaps;

        static SingleTextureEntityDrawer()
        {
            Bitmaps = new Dictionary<string, Bitmap>();
            var imagesDirectory = new DirectoryInfo("Textures/Entities");
            foreach (var fileInfo in imagesDirectory.GetFiles("*.png"))
                Bitmaps[fileInfo.Name] = (Bitmap) Image.FromFile(fileInfo.FullName);
        }

        public static void DrawEntity(Graphics graphics, Entity entity, bool isDebugMode = false)
        {
            var texture = entity as ISingleTexture;
            if (texture == null) return;
            graphics.TranslateTransform(entity.X, entity.Y);
            graphics.RotateTransform(entity.Direction);

            if (Bitmaps.ContainsKey(texture.GetTextureFileName()))
            {
                var bitmap = Bitmaps[texture.GetTextureFileName()];
                DrawUtils.DrawCenteredBitmap(graphics, bitmap);
            }
            if (isDebugMode)
                DrawUtils.DrawCollisionBox(graphics, entity);

            graphics.RotateTransform(-entity.Direction);
            graphics.TranslateTransform(-entity.X, -entity.Y);
        }
    }
}