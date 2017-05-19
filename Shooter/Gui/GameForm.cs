using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Shooter.Gui
{
    class GameForm : Form
    {
        private Game game;
        private Timer gameTimer;
        private bool wasdKeyLastPressed;
        private bool debugMode;
        private bool isGameActive;

        private ControlsHelpDrawer controlsHelpDrawer;
        private BackgroundDrawer bgDrawer;

        public GameForm()
        {
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
            StartNewGame();
        }


        private void StartNewGame()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            gameTimer?.Stop();
            isGameActive = true;
            game = new Game(ClientSize.Width, ClientSize.Height,true);
            game.GameOver += () => isGameActive = false;
            bgDrawer = new BackgroundDrawer(ClientSize.Width, ClientSize.Height);
            controlsHelpDrawer = new ControlsHelpDrawer(5000);
            gameTimer = new Timer {Interval = 10};
            gameTimer.Tick += (sender, args) =>
            {
                controlsHelpDrawer?.DecreaseTime(gameTimer.Interval);
                game.GameTick();
                bgDrawer.Tick();
                Invalidate();
            };
            gameTimer.Start();
            wasdKeyLastPressed = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (game == null) return;
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
            if (game == null) return;
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
            switch (keyCode)
            {
                case Keys.W:
                case Keys.A:
                case Keys.S:
                case Keys.D:
                    wasdKeyLastPressed = true;
                    break;
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                case Keys.Right:
                    wasdKeyLastPressed = false;
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
            if (isGameActive)
            {
                DrawGameEntities(graphics);
                controlsHelpDrawer.DrawControlsHelp(graphics, game.Player, wasdKeyLastPressed);
                DrawGuiOverlay(graphics);
                if (gameTimer != null && !gameTimer.Enabled)
                    DrawPauseScreen(graphics);
            }
            else
                DrawGameOverScreen(graphics);
        }

        private void DrawGameOverScreen(Graphics graphics)
        {
            graphics.DrawString(
                $"Игра окончена. Очки: {game.Score}",
                new Font("Courier", 16),
                Brushes.White,
                new Rectangle(new Point(0, 0), ClientSize),
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
        }

        private void DrawGameEntities(Graphics graphics)
        {
            foreach (var entity in game.GetEntities)
                entity.GetDrawingAction()?.Invoke(graphics, entity, debugMode);
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

        private void DrawGuiOverlay(Graphics graphics)
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