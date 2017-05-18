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
                graphics.DrawImage(bitmap, -bitmap.Width / 2, -bitmap.Height / 2);
            }
            if (isDebugMode)
                DrawCollisionBox(graphics, entity);

            graphics.RotateTransform(-entity.Direction);
            graphics.TranslateTransform(-entity.X, -entity.Y);
        }

        internal static void DrawCollisionBox(Graphics graphics, Entity entity)
        {
            Rectangle rect;
            Pen pen;
            if (entity.CollisionBox == null)
            {
                rect = new Rectangle(-32, -32, 64, 64);
                pen = entity is Player ? Pens.GreenYellow : Pens.Pink;
            }
            else
            {
                var cb = entity.CollisionBox;
                rect = new Rectangle(-(int) (cb.Width / 2), -(int) (cb.Height / 2), (int) cb.Width, (int) cb.Height);
                pen = entity is Player ? Pens.GreenYellow : Pens.Pink;
            }
            graphics.DrawRectangle(pen, rect);
        }
    }
}