using System.Drawing;

namespace Shooter.Gui
{
    public static class DrawUtils
    {
        public static void DrawCenteredBitmap(Graphics graphics, Bitmap bitmap)
        {
            graphics.DrawImage(bitmap, -bitmap.Width / 2, -bitmap.Height / 2);
        }

        public static void DrawCollisionBox(Graphics graphics, Entity entity)
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
                pen = entity is Player ? Pens.Green : Pens.Red;
            }
            graphics.DrawRectangle(pen, rect);
        }
    }
}