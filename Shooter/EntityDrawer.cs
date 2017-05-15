using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Shooter
{
    public class EntityDrawer
    {
        private readonly Dictionary<string, Bitmap> bitmaps;

        public EntityDrawer()
        {
            bitmaps = new Dictionary<string, Bitmap>();
            var imagesDirectory = new DirectoryInfo("Textures/Entities");
            foreach (var fileInfo in imagesDirectory.GetFiles("*.png"))
                bitmaps[fileInfo.Name] = (Bitmap) Image.FromFile(fileInfo.FullName);
        }

        public void DrawEntity(Graphics graphics, Entity entity, bool isDebugMode = false)
        {
            graphics.TranslateTransform(entity.X, entity.Y);
            graphics.RotateTransform(entity.Direction);

            if (bitmaps.ContainsKey(entity.GetTextureFileName()))
            {
                var bitmap = bitmaps[entity.GetTextureFileName()];
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