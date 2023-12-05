using System;
using System.IO;

namespace L31_BraveNewWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "map.txt";

            // Для тестирования можно раскоментировать этот участок и закоментировать загрузку карты из файла
            /*            char[,] map =
                        {
                                                    { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#','X','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ','#','#','#',' ',' ',' ',' ','#' },
                                                    { '#',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','X','#',' ',' ',' ',' ','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ','#',' ','#',' ',' ',' ',' ','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ','#',' ','#',' ',' ',' ',' ','#' },
                                                    { '#',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#',' ','#','#','#','#','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#',' ',' ',' ',' ','X','#',' ',' ',' ',' ',' ',' ',' ',' ','X',' ','#' },
                                                    { '#',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#' },
                                                    { '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#' },
                        };*/
            char[,] map = ReadMap(path);
            char[] bag = new char[0];

            char wall = '#';
            char player = '@';
            char item = 'X';
            char remainSymbol = 'O';

            int userX = 1;
            int userY = 1;
            int bagX = 0;
            int bagY = map.GetLength(1);

            bool isOpen = true;

            Console.CursorVisible = false;

            while (isOpen)
            {
                DrawBag(bag, bagX, bagY);
                DrawMap(map);

                Console.SetCursorPosition(userX, userY);
                Console.Write(player);
                UpdatePlayerState(map, ref userX, ref userY, wall);

                if (map[userX, userY] == item)
                    bag = PickUpItem(map, userX, userY, bag, item, remainSymbol);
            }
        }

        static int[] GetDirection(ConsoleKeyInfo pressKey)
        {
            int[] position = new int[2];

            switch (pressKey.Key)
            {
                case ConsoleKey.LeftArrow:
                    position[0] = -1;
                    break;

                case ConsoleKey.RightArrow:
                    position[0] = 1;
                    break;

                case ConsoleKey.UpArrow:
                    position[1] = -1;
                    break;

                case ConsoleKey.DownArrow:
                    position[1] = 1;
                    break;
            }

            return position;
        }

        static char[] PickUpItem(char[,] map, int positionX, int positionY, char[] bag, char pickUpItem, char remainSymbol)
        {
            map[positionX, positionY] = remainSymbol;
            char[] tempBag = new char[bag.Length + 1];

            for (int i = 0; i < bag.Length; i++)
                tempBag[i] = bag[i];

            tempBag[bag.Length] = pickUpItem;
            return tempBag;
        }

        static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);
            char[,] map = new char[GetMaxLenghtOfLines(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];

            return map;
        }

        static void DrawMap(char[,] map)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                    Console.Write(map[x, y]);

                Console.WriteLine();
            }
        }

        static void DrawBag(char[] bag, int positionX, int positionY)
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.Write("Сумка: ");

            for (int i = 0; i < bag.Length; i++)
                Console.Write(bag[i] + " ");
        }

        static int GetMaxLenghtOfLines(string[] lines)
        {
            int maxLenght = lines[0].Length;

            for (int i = 1; i < lines.Length; i++)
                if (lines[i].Length > maxLenght)
                    maxLenght = lines[i].Length;

            return maxLenght;
        }

        static void UpdatePlayerState(char[,] map, ref int positionX, ref int positionY, char wall)
        {
            ConsoleKeyInfo pressKey = Console.ReadKey();
            int[] direction = GetDirection(pressKey);

            if (CanMovedPlayer(map, positionX + direction[0], positionY + direction[1], wall))
                MovePlayer(ref positionX, ref positionY, direction);
        }

        private static void MovePlayer(ref int positionX, ref int positionY, int[] direction)
        {
            positionX += direction[0];
            positionY += direction[1];
        }

        static bool CanMovedPlayer(char[,] map, int positionX, int positionY, char wall)
        {
            return map[positionX, positionY] != wall;
        }
    }
}

