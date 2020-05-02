using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeWaste
{
    class GameForm : Form
    {
        private SnakeGame game;
        private bool fail = false; 
        public GameForm(Point snake, Direction direction, int w, int h)
        {
            DoubleBuffered = true;
            game = new SnakeGame(snake, direction, w, h);
            var timer = new Timer();
            timer.Interval = 200;
            KeyPress += (sender, args) =>
            {
                switch (args.KeyChar)
                {
                    case 'w':
                        game.Snake.ChangeDirection(Direction.Up);
                        break;
                    case 's':
                        game.Snake.ChangeDirection(Direction.Down);
                        break;
                    case 'a':
                        game.Snake.ChangeDirection(Direction.Left);
                        break;
                    case 'd':
                        game.Snake.ChangeDirection(Direction.Right);
                        break;
                }
            };
            timer.Tick += (sender, args) =>
            {
                if (!game.MakeTurn())
                {
                    timer.Stop();
                    fail = true;
                }
                Invalidate();
            };
            timer.Start();
            Paint += (sender, args) =>
            {
                Draw(args);
            };
        }

        private void Draw(PaintEventArgs args)
        {
            var s = Math.Min(ClientSize.Width, ClientSize.Height);
            args.Graphics.FillEllipse(Brushes.Red, GetRect(game.Apple));
            foreach (var part in game.Snake.Tail)
                args.Graphics.FillEllipse(Brushes.Green, GetRect(part));
            if (fail)
                args.Graphics.DrawString("You lose!", new Font("Impact", 16), Brushes.Black, new Point(150, 150));
        }
        
        private Rectangle GetRect(Point p)
        {
            var s = new Point(ClientSize.Width / game.width * p.X,
                          ClientSize.Height / game.height * p.Y);
            return new Rectangle(s, new Size(ClientSize.Width / game.width,
                                             ClientSize.Height / game.height));
        }
    }
}
