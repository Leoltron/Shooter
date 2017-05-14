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
        private Timer gameTimer;
        private bool IsGameActive = false;
        private readonly EnemyDrawer enemyDrawer;

        //private Button btn;

        public GameForm()
        {
            /*btn = new Button();
            btn.Size = new Size(ClientSize.Width, ClientSize.Height);
            btn.Click += (s, a) =>
            {
                n = !n;
                FormBorderStyle = n ? FormBorderStyle.None : FormBorderStyle.Sizable;
                MaximizeBox = !n;
            };
            Controls.Add(btn);*/
            Text = "Shooter";
            DoubleBuffered = true;
            MinimumSize = new Size(400, 400);
            Size = new Size(500, 500);
            KeyDown += (sender, keyArgs) => HandleMoveKey(keyArgs.KeyCode, true);
            KeyUp += (sender, keyArgs) => HandleMoveKey(keyArgs.KeyCode, false);
            enemyDrawer = new EnemyDrawer();
            StartNewGame();
        }

        private void StartNewGame()
        {
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            gameTimer?.Stop();
            game = new Game(ClientSize.Width, ClientSize.Height);
            gameTimer = new Timer {Interval = 50};
            gameTimer.Tick += (sender, args) =>
            {
                game.GameTick();
                Invalidate();
            };
            gameTimer.Start();
            IsGameActive = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (game == null || !IsGameActive) return;
            if (!gameTimer.Enabled)
                gameTimer.Start();
            else
                switch (char.ToLower(e.KeyChar))
                {
                    case ' ':
                        game.Fire();
                        break;
                    case 'p':
                        gameTimer.Stop();
                        Invalidate();
                        break;
                }
        }

        private void HandleMoveKey(Keys keyCode, bool isKeyDown)
        {
            if (game == null || !IsGameActive) return;
            switch (keyCode)
            {
                case Keys.W:
                case Keys.Up:
                    game.SetPlayerGoingUp(isKeyDown);
                    break;
                case Keys.A:
                case Keys.Left:
                    game.SetPlayerGoingLeft(isKeyDown);
                    break;
                case Keys.S:
                case Keys.Down:
                    game.SetPlayerGoingDown(isKeyDown);
                    break;
                case Keys.D:
                case Keys.Right:
                    game.SetPlayerGoingRight(isKeyDown);
                    break;
            }
        }


        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            gameTimer?.Stop();
            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            base.OnPaint(eventArgs);
            var graphics = eventArgs.Graphics;
            foreach (var entity in game.GetEntities)
                enemyDrawer.DrawEntity(graphics, entity);
            DrawGui(graphics);
            if (gameTimer != null && !gameTimer.Enabled)
                DrawPauseScreen(graphics);
        }

        private void DrawPauseScreen(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.Black)),
                0, 0, ClientSize.Width, ClientSize.Height);
            graphics.DrawString(
                "Пауза. Нажмите любую клавишу для продолжения",
                new Font("Courier", 16),
                Brushes.White,
                new Rectangle(new Point(0, 0), ClientSize),
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
        }

        private void DrawGui(Graphics graphics)
        {
            graphics.DrawString(
                $"Score: {game.Score}",
                new Font("Courier", 16),
                Brushes.GreenYellow,
                new Rectangle(new Point(0, 0), ClientSize),
                new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Near
                });
            graphics.DrawString(
                $"HP: {game.Health}",
                new Font("Courier", 16),
                Brushes.Red,
                new Rectangle(new Point(0, 0), ClientSize),
                new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                });
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, ClientSize.Height);
        }
    }
}