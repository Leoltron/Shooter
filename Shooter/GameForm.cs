using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Shooter
{
    class GameForm : Form
    {
        private Game game;
        private Timer gameTimer;
        private bool IsGameActive;
        private bool debugMode;

        private readonly EntityDrawer entityDrawer;
        private readonly PlayerDrawer playerDrawer;

        private BackgroundDrawer bgDrawer;

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
            Size = new Size(750, 750);
            KeyDown += (sender, keyArgs) =>
            {
                if (keyArgs.KeyCode == Keys.D && keyArgs.Modifiers == Keys.Control)
                    debugMode = !debugMode;
                else
                    HandleMoveKey(keyArgs.KeyCode, true);
            };
            KeyUp += (sender, keyArgs) => HandleMoveKey(keyArgs.KeyCode, false);
            entityDrawer = new EntityDrawer();
            playerDrawer = new PlayerDrawer();
            StartNewGame();
        }


        private void StartNewGame()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            gameTimer?.Stop();
            game = new Game(ClientSize.Width, ClientSize.Height);
            bgDrawer = new BackgroundDrawer(ClientSize.Width, ClientSize.Height);
            gameTimer = new Timer {Interval = 10};
            gameTimer.Tick += (sender, args) =>
            {
                game.GameTick();
                bgDrawer.Tick();
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
            DrawGameEntities(graphics);
            DrawGui(graphics);
            if (gameTimer != null && !gameTimer.Enabled)
                DrawPauseScreen(graphics);
        }

        private void DrawGameEntities(Graphics graphics)
        {
            foreach (var entity in game.GetEntities)
                if (entity is Player)
                    playerDrawer.DrawPlayer((Player)entity, graphics, debugMode);
                else
                    entityDrawer.DrawEntity(graphics, entity, debugMode);
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
                $"HP: {game.Player.Health}",
                new Font("Courier", 16),
                Brushes.Red,
                new Rectangle(new Point(0, 0), ClientSize),
                new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                });
            if (debugMode)
                graphics.DrawString(
                    game.Player.ToString(),
                    new Font("Courier", 10),
                    Brushes.MediumSpringGreen,
                    new Rectangle(new Point(0, 0), ClientSize),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Far
                    });
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            bgDrawer?.DrawBackground(e.Graphics);
        }
    }
}