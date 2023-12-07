using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class SnakeGame
{
    private const int Width = 20;
    private const int Height = 10;

    private List<Point> snake;
    private Direction direction;
    private Point food;
    private bool isGameOver;

    public SnakeGame()
    {
        snake = new List<Point>
        {
            new Point(0, 0)
        };

        direction = Direction.Right;
        GenerateFood();
        isGameOver = false;
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Console.Clear();

        while (!isGameOver)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                HandleInput(key);
            }

            Move();
            CheckCollision();
            Draw();
            Thread.Sleep(100); // задержка для управления скоростью змейки
        }

        Console.Clear();
        Console.WriteLine("Game Over!");
    }

    private void HandleInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (direction != Direction.Down)
                    direction = Direction.Up;
                break;

            case ConsoleKey.DownArrow:
                if (direction != Direction.Up)
                    direction = Direction.Down;
                break;

            case ConsoleKey.LeftArrow:
                if (direction != Direction.Right)
                    direction = Direction.Left;
                break;

            case ConsoleKey.RightArrow:
                if (direction != Direction.Left)
                    direction = Direction.Right;
                break;
        }
    }

    private void Move()
    {
        Point head = snake.First();

        Point newHead = direction switch
        {
            Direction.Up => new Point(head.X, (head.Y - 1 + Height) % Height),
            Direction.Down => new Point(head.X, (head.Y + 1) % Height),
            Direction.Left => new Point((head.X - 1 + Width) % Width, head.Y),
            Direction.Right => new Point((head.X + 1) % Width, head.Y),
            _ => head,
        };

        snake.Insert(0, newHead);

        if (newHead.Equals(food))
        {
            GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    private void CheckCollision()
    {
        Point head = snake.First();

        if (snake.Count > 1 && snake.Skip(1).Any(segment => segment.Equals(head)))
        {
            isGameOver = true;
        }

        if (head.X < 0 || head.X >= Width || head.Y < 0 || head.Y >= Height)
        {
            isGameOver = true;
        }
    }

    private void GenerateFood()
    {
        Random random = new Random();
        do
        {
            food = new Point(random.Next(Width), random.Next(Height));
        } while (snake.Contains(food));
    }

    private void Draw()
    {
        Console.Clear();

        // Рисуем змейку
        foreach (Point segment in snake)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("*");
        }

        // Рисуем еду
        Console.SetCursorPosition(food.X, food.Y);
        Console.Write("#");
    }

    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}

class Program
{
    static void Main()
    {
        SnakeGame snakeGame = new SnakeGame();
        snakeGame.Run();
    }
}
