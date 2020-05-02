using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TimeWaste
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public class Snake
    {
        private Point head;

        public Point Head { get => head; private set => head = value; }
        public Direction Direction { get; private set; }
        public List<Point> Tail { get; private set; }
        public Snake(Point head, Direction direction)
        {
            Head = head;
            Direction = direction;
            switch (Direction)
            {
                case Direction.Up:
                    Tail = new List<Point> { Head, new Point(Head.X, Head.Y + 1), new Point(Head.X, Head.Y + 2) };
                    break;
                case Direction.Down:
                    Tail = new List<Point> { Head, new Point(Head.X, Head.Y - 1), new Point(Head.X, Head.Y - 2) };
                    break;
                case Direction.Right:
                    Tail = new List<Point> { Head, new Point(Head.X - 1, Head.Y), new Point(Head.X - 2, Head.Y) };
                    break;
                case Direction.Left:
                    Tail = new List<Point> { Head, new Point(Head.X + 1, Head.Y), new Point(Head.X + 2, Head.Y) };
                    break;
            }
        }
        
        public Point GetNewHead()
        {
            var newHead = head;
            switch (Direction)
            {
                case Direction.Up:
                    newHead.Y -= 1;
                    break;
                case Direction.Down:
                    newHead.Y += 1;
                    break;
                case Direction.Left:
                    newHead.X -= 1;
                    break;
                case Direction.Right:
                    newHead.X += 1;
                    break;
            }
            return newHead;
        }
        public bool Move()
        {
            Head = GetNewHead();
            if (Tail.Contains(Head))
                return false;
            var newTail = new List<Point> { Head };
            for (var i = 0; i < Tail.Count - 1; i++)
                newTail.Add(Tail[i]);
            Tail = newTail;
            return true;
        }

        public void Grow()
        {
            var oldEnd = Tail[Tail.Count - 1];
            Move();
            Tail.Add(oldEnd);
        }

        public void ChangeDirection(Direction newDirection)
        {
            switch(Direction)
            {
                case Direction.Up:
                    if (newDirection != Direction.Down)
                        Direction = newDirection;
                    break;
                case Direction.Down:
                    if (newDirection != Direction.Up)
                        Direction = newDirection;
                    break;
                case Direction.Left:
                    if (newDirection != Direction.Right)
                        Direction = newDirection;
                    break;
                case Direction.Right:
                    if (newDirection != Direction.Left)
                        Direction = newDirection;
                    break;
            }
        }
    }

    public class SnakeGame
    {
        public int width;
        public int height;
        private Random random;
        public Snake Snake { get; private set; }
        public Point Apple { get; private set; }

        public SnakeGame(Point snake, Direction direction, int w, int h)
        {
            Snake = new Snake(snake, direction);
            width = w;
            height = h;
            random = new Random();
        }

        private void GenerateApple()
        {
            var newPos = new Point(random.Next(0, width), random.Next(0, height));
            while (Snake.Tail.Contains(newPos))
                newPos = new Point(random.Next(0, width), random.Next(0, height));
            Apple = newPos;
        }

        public bool MakeTurn()
        {
            if (Snake.GetNewHead() == Apple)
            {
                Snake.Grow();
                GenerateApple();
            }
            else
                if (!Snake.Move())
                    return false;
            if (Snake.Head.Y < 0 || Snake.Head.Y >= height)
                return false;
            if (Snake.Head.X < 0 || Snake.Head.X >= width)
                return false;
            return true;
        }
    }
}
