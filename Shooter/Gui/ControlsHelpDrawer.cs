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
        private int TimeRemain;

        public ControlsHelpDrawer(int showTime)
        {
            TimeRemain = showTime;
            keysBitmaps = new Dictionary<Keys, Bitmap>();
            var imagesDirectory = new DirectoryInfo("Textures\\Controls\\");
            foreach (var key in wasdControls.Concat(arrowControls))
                keysBitmaps[key] = (Bitmap) Image.FromFile($"{imagesDirectory.FullName}{key}.png");
        }

        public void DecreaseTime(int dtime)
        {
            TimeRemain -= dtime;
        }

        public void DrawControlsHelp(Graphics graphics, Player player, bool wasdButtonLastPushed)
        {
            if (TimeRemain <= 0) return;
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
                    player.CollisionBox.Left- leftBitmap.Width,
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