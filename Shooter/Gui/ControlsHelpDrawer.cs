using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooter.Gui
{
    class ControlsHelpDrawer
    {
        private readonly Dictionary<Keys, Bitmap> keysBitmaps;
        private readonly Keys[] wasdControls = {Keys.W, Keys.A, Keys.S, Keys.D};
        private readonly Keys[] arrowControls = {Keys.Up, Keys.Left, Keys.Down, Keys.Right};
        private int timeRemain;

        public ControlsHelpDrawer(int showTime)
        {
            timeRemain = showTime;
            keysBitmaps = new Dictionary<Keys, Bitmap>();
            var imagesDirectory = new DirectoryInfo("Textures\\Controls\\");
            foreach (var key in wasdControls.Concat(arrowControls))
                keysBitmaps[key] = (Bitmap) Image.FromFile($"{imagesDirectory.FullName}{key}.png");
        }

        public void DecreaseTime(int dtime)
        {
            timeRemain -= dtime;
        }

        public void DrawControlsHelp(Graphics graphics, Player player, bool wasdButtonLastPushed)
        {
            if (timeRemain <= 0) return;
            const float textBoxHeight = 20;
            const float textBoxWidth = 256;
            graphics.DrawString("Пробел - стрельба", new Font("Courier", 16),
                new SolidBrush(Color.FromArgb(56, 228, 255)),
                new RectangleF(player.X - textBoxWidth / 2,
                    player.CollisionBox.Top - 64 - textBoxHeight,
                    textBoxWidth,
                    textBoxHeight),
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                }
            );
            var controls = wasdButtonLastPushed ? wasdControls : arrowControls;
            var upBitmap = keysBitmaps[controls[0]];
            graphics.DrawImage(upBitmap,
                new PointF(
                    player.CollisionBox.Left,
                    player.CollisionBox.Top - upBitmap.Height
                )
            );

            var leftBitmap = keysBitmaps[controls[1]];
            graphics.DrawImage(leftBitmap,
                new PointF(
                    player.CollisionBox.Left - leftBitmap.Width,
                    player.CollisionBox.Top
                )
            );

            var downBitmap = keysBitmaps[controls[2]];
            graphics.DrawImage(downBitmap,
                new PointF(
                    player.CollisionBox.Left,
                    player.CollisionBox.Bottom
                )
            );

            var rightBitmap = keysBitmaps[controls[3]];
            graphics.DrawImage(rightBitmap,
                new PointF(
                    player.CollisionBox.Right,
                    player.CollisionBox.Top
                )
            );
        }
    }
}