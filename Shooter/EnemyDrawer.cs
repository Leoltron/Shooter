using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Shooter
{
    public class EnemyDrawer
    {
        private readonly Dictionary<string, Bitmap> bitmaps;

        public EnemyDrawer()
        {
            bitmaps = new Dictionary<string, Bitmap>();
            var imagesDirectory = new DirectoryInfo("Textures/Enemies");
            foreach (var fileInfo in imagesDirectory.GetFiles("*.png"))
                bitmaps[fileInfo.Name] = (Bitmap) Image.FromFile(fileInfo.FullName);
        }

        public void DrawEntity(Graphics graphics, Entity entity)
        {
            if (!bitmaps.ContainsKey(entity.GetTextureFileName())) return;
            var bitmap = bitmaps[entity.GetTextureFileName()];
            graphics.TranslateTransform(entity.X, entity.Y);
            graphics.RotateTransform(entity.Direction);
            graphics.DrawImage(bitmap, -bitmap.Width / 2, -bitmap.Height / 2);
            graphics.RotateTransform(-entity.Direction);
            graphics.TranslateTransform(-entity.X, -entity.Y);
        }
    }
}