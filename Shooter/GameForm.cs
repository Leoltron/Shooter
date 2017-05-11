using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooter
{
    class GameForm : Form
    {
        private Game game;
        private bool n;

        private Button btn;

        public GameForm()
        {
            btn = new Button();
            btn.Size = new Size(ClientSize.Width, ClientSize.Height);
            btn.Click += (s, a) =>
            {
                n = !n;
                FormBorderStyle = n ? FormBorderStyle.None : FormBorderStyle.Sizable;
                MaximizeBox = !n;
            };
            Controls.Add(btn);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            btn.Size = new Size(ClientSize.Width, ClientSize.Height);
        }
    }
}
