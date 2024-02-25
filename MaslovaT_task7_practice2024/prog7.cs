using System;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace MaslovaT_task7_practice2024
{
    internal static class prog7
    {
        static char playerIcon = '█';

        /// <summary>
        /// More convenient Console.WriteLine
        /// </summary>
        static void Writeln(object input)
        {
            Console.WriteLine(input);
        }
        /// <summary>
        /// More convenient Console.Write
        /// </summary>
        static void Write(object input)
        {
            Console.Write(input);
        }
        #region     
        /// <summary>
        /// The list that contains labyrinth data
        /// </summary>
        static List<LabyrinthString> labyrinthDataList = new List<LabyrinthString>();
        /// <summary>
        /// Player coordinates
        /// </summary>
        static Player player = new Player();

        /// <summary>
        /// Read the labyrinth file and put it into the list
        /// </summary>
        static bool ReadLabyrinthFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string @iterator;
                while ((@iterator = sr.ReadLine()) != null)
                {
                    if (@iterator != string.Empty)
                    {
                        labyrinthDataList.Add(new LabyrinthString(@iterator));
                    }
                }
                sr.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ошибка чтения файла лабиринта " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Render the labyrinth
        /// </summary>
        static void RenderLabyrinth()
        {
            for (int i = 0; i < labyrinthDataList.Count; i++)
            {
                for (int j = 0; j < labyrinthDataList[i].chars.Length; j++)
                {
                    if (labyrinthDataList[i].chars[j] == 'ы')
                        Write(' ');
                    else
                        Write(labyrinthDataList[i].chars[j]);
                }
                Writeln(" ");
            }
        }
        /// <summary>
        /// Render the labyrinth with the path
        /// </summary>
        static void RenderLabyrinthCheat()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < labyrinthDataList.Count; i++)
            {
                for (int j = 0; j < labyrinthDataList[i].chars.Length; j++)
                {
                    if (labyrinthDataList[i].chars[j] == 'ы')
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    Write(labyrinthDataList[i].chars[j]);
                }
                Writeln(" ");
            }
        }
        /// <summary>
        /// Render the player
        /// </summary>
        static void RenderPlayer(char image)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(player.X, player.Y);
            Console.Write(image);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Erase the player icon
        /// </summary>
        public static void ErasePlayer()
        {
            RenderPlayer(' ');
        }
        #endregion

        public static bool PlayerHitEnemy(Enemy enemy)
        {
            if (player.X == enemy.X && player.Y == enemy.Y)
            {
                return true;
            }
            return false;
        }

        static void drawHPbar(int HP)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(labyrinthDataList[1].chars.Length + 10, 1);
            Write("HP [");
            for (int i = 0; i < HP; i++)
            {
                Write("#");
            }
            for (int i = 0; i < 10 - HP; i++)
            {
                Write("_");
            }
            Writeln("]");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Main()
        {
            Console.CursorVisible = false;
            if (!ReadLabyrinthFile(Directory.GetCurrentDirectory() + @"\labyrinth.txt"))
            {
                Console.ReadKey();
                return;
            }

            RenderLabyrinth();

            bool stillPlaying = true;
            RenderPlayer(playerIcon);

            // Enemy generation
            List<Enemy> enemyList = new List<Enemy>();
            for (int i = 0; i < 12; i++)
            {
                enemyList.Add(new Enemy(labyrinthDataList));
                enemyList[i].RenderEnemy(enemyList[i].image);
            }


            drawHPbar(player.HP);

            while (stillPlaying)
            {
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.UpArrow:
                        player.Move(0, -1, labyrinthDataList);
                        break;
                    case ConsoleKey.DownArrow:
                        player.Move(0, 1, labyrinthDataList);
                        break;
                    case ConsoleKey.LeftArrow:
                        player.Move(-1, 0, labyrinthDataList);
                        break;
                    case ConsoleKey.RightArrow:
                        player.Move(1, 0, labyrinthDataList);
                        break;
                    case ConsoleKey.X:
                        stillPlaying = false;
                        break;
                    case ConsoleKey.S:
                        RenderLabyrinthCheat();
                        break;
                    default:
                        break;

                }
                RenderPlayer(playerIcon);

                // update enemies
                for (int i = 0; i < enemyList.Count; i++)
                {
                    Random rnd = new Random();

                    enemyList[i].EraseEnemy();
                    enemyList[i].Move(rnd.Next(-1, 2), rnd.Next(-1, 2), labyrinthDataList);
                    enemyList[i].RenderEnemy(enemyList[i].image);

                    if (PlayerHitEnemy(enemyList[i]))
                    {
                        player.HP--;
                        drawHPbar(player.HP);
                        if (player.HP == 0)
                        {
                            stillPlaying = false;
                            Console.SetCursorPosition(0, labyrinthDataList.Count + 1);

                            Console.ForegroundColor = ConsoleColor.Red;
                            Writeln("ПОРАЖЕНИЕ");

                            Writeln("Игра окончена");
                            Writeln("Нажмите любую клавишу для выхода");
                            Console.ReadKey();

                            return;
                        }
                    }

                }

                if (player.Y >= labyrinthDataList.Count - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    stillPlaying = false;
                    Console.SetCursorPosition(0, labyrinthDataList.Count + 1);
                    Writeln("ПОБЕДА");
                }
            }

            Console.SetCursorPosition(0, labyrinthDataList.Count + 2);
            Writeln("Игра окончена");
            Writeln("Нажмите любую клавишу для выхода");
            Thread.Sleep(5000);
            Console.ReadKey();
        }
    }
    /// <summary>
    /// Координаты игрока
    /// </summary>
    class Player
    {
        /// <summary>
        /// Coordinates
        /// </summary>
        public int X, Y;
        public int HP;
        /// <summary>
        /// Base constructor
        /// </summary>
        public Player()
        {
            X = 1; Y = 1; HP = 10;
        }
        /// <summary>
        /// Change the position of the player
        /// </summary>
        public void Move(int deltaX, int deltaY, List<LabyrinthString> dataList)
        {
            if (PlayerCanMove(deltaX, deltaY, dataList))
            {
                prog7.ErasePlayer();
                X += deltaX;
                Y += deltaY;
            }

        }
        /// <summary>
        /// Определить, возможно ли движение
        /// </summary>
        bool PlayerCanMove(int deltaX, int deltaY, List<LabyrinthString> dataList)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (newX < 0 || newY < 0)
            {
                return false;
            }
            if (newY < dataList.Count)
                if (newX < dataList[newY].chars.Length)
                    if (dataList[newY].chars[newX] != ' ' && dataList[newY].chars[newX] != 'ы')
                    {
                        return false;
                    }
            return true;
        }
    }
    /// <summary>
    /// Одна строчка лабиринта
    /// </summary>
    class LabyrinthString
    {
        public char[] chars = new char[128];
        public LabyrinthString(string input)
        {
            chars = input.ToCharArray();
        }
        public override string ToString()
        {
            return new string(chars);
        }
    }
    /// <summary>
    /// Обработка врагов
    /// </summary>
    class Enemy
    {
        public char image;
        public int X, Y;
        /// <summary>
        /// Base constructor
        /// </summary>
        public Enemy(List<LabyrinthString> dataList)
        {
            Random rnd = new Random();

            int tempY = rnd.Next(1, dataList.Count - 3);
            int tempX = rnd.Next(1, dataList[tempY].chars.Length);
            while (dataList[tempY].chars[tempX] != ' ')
            {
                tempY = rnd.Next(1, dataList.Count - 3);
                tempX = rnd.Next(1, dataList[tempY].chars.Length);
            }
            X = tempX; Y = tempY;

            image = '▀';
        }
        /// <summary>
        /// Render Enemy
        /// </summary>
        public void RenderEnemy(char image)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(X, Y);
            Console.Write(image);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void EraseEnemy()
        {
            RenderEnemy(' ');
        }
        /// <summary>
        /// Change the position of the enemy
        /// </summary>
        public void Move(int deltaX, int deltaY, List<LabyrinthString> dataList)
        {
            if (EnemyCanMove(deltaX, deltaY, dataList))
            {
                EraseEnemy();
                X += deltaX;
                Y += deltaY;
            }

        }
        /// <summary>
        /// Определить, возможно ли движение
        /// </summary>
        bool EnemyCanMove(int deltaX, int deltaY, List<LabyrinthString> dataList)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (newX < 0 || newY < 0)
            {
                return false;
            }
            if (newY < dataList.Count)
                if (newX < dataList[newY].chars.Length)
                    if (dataList[newY].chars[newX] != ' ' && dataList[newY].chars[newX] != 'ы')
                    {
                        return false;
                    }
            return true;
        }
    }
}