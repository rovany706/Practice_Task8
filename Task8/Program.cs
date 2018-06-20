using System;
using System.Collections.Generic;
using System.Linq;
using InputHelper;

namespace Task8
{
    class Program
    {
        private static bool[] visitedNodes; //массив для хранения информации о прохождении по графу
        private static int[,] matrix; //матрица смежности
        private static Stack<char> path = new Stack<char>(); //стэк для хранения пути прохождения по графу
        private static string foundCycle; //найденный цикл
        private static bool isFound = false;
        private static Random rand = new Random(); //ДСЧ

        //проверка правильности введенной матрицы смежности
        static bool CheckMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, i] != 0)
                    return false;
                for (int j = 0; j < matrix.GetLength(0); j++)
                    if (matrix[i, j] != matrix[j, i])
                        return false;
            }

            return true;
        }

        //Depth-first search - поиск в глубину
        static void DFS(int n, int vert, int startNode)
        {
            path.Push((char)('A' + vert));
            visitedNodes[vert] = true;
            if (n == 0)
            {
                visitedNodes[vert] = false;

                if (matrix[vert, startNode] == 1)
                {
                    isFound = true;
                    int cycleLength = path.Count;
                    for (int i = 0; i < cycleLength; i++)
                        foundCycle += path.Pop();
                    return;
                }
                else
                    return;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (!isFound)
                {
                    if (visitedNodes[i] == false && matrix[vert, i] == 1)
                    {
                        DFS(n - 1, i, startNode);
                        if (!isFound)
                            path.Pop();
                    }
                }
            }
            visitedNodes[vert] = false;

        }

        static void Main()
        {
            Console.Clear();
            PrintTask();
            Console.WriteLine("1. Сгенерировать случайный граф\n2. Ввести матрицу смежности графа\n3. Выйти из программы");
            int input = Input.ReadInt("Выберите действие: ", 1, 3);

            switch (input)
            {
                case 1:
                    RandomGraph();
                    break;
                case 2:
                    ManualInput();
                    break;
                case 3:
                    return;
            }

        }

        static void PrintTask()
        {
            Console.WriteLine("Задача 8\n=================");
            Console.WriteLine("Условие задачи:\nГраф задан матрицей смежности.\n" +
                              "Найти в нем какой-либо простой цикл из K вершин.\n" +
                              "=================");
        }

        static void ManualInput()
        {
            foundCycle = string.Empty;
            Console.Clear();
            PrintTask();

            int n = Input.ReadInt("Введите количество вершин в графе: ", 3);
            matrix = new int[n, n];
            visitedNodes = new bool[n];

            Console.WriteLine($"Вводите матрицу смежности построчно (всего {n} строк), разделяя каждый элемент пробелом ({n} элементов в каждой строке):");
            bool ok;
            do
            {
                ok = true;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    int num;
                    do
                    {
                        string[] inputLine = Console.ReadLine().Trim().Split();
                        if (inputLine.Length != matrix.GetLength(0)) //проверка длины строки
                        {
                            Console.WriteLine("Ошибка! Повторите ввод строки.");
                            ok = false;
                        }
                        else
                            ok = true;

                        if (ok)
                        {
                            for (int j = 0; j < matrix.GetLength(0); j++) //проверка правильности ввода
                            {
                                ok = int.TryParse(inputLine[j], out num);
                                if (!ok || num != 1 && num != 0)
                                {
                                    ok = false;
                                    Console.WriteLine("Ошибка! Повторите ввод строки.");
                                    break;
                                }

                                matrix[i, j] = num;
                            }
                        }
                    } while (!ok);
                }

                ok = CheckMatrix(matrix);
                if (!ok)
                    Console.WriteLine("Ошибка! В введенной матрице есть ошибки. Повторите ввод матрицы.");
            } while (!ok);

            PrintMatrix();

            int k = Input.ReadInt("Введите длину цикла, который хотите найти: ", 3, n);
            for (int i = 0; i < n - (k - 1); i++)
            {
                DFS(k - 1, i, i);
                if (!isFound)
                {
                    path.Pop();
                    visitedNodes[i] = true;
                }
                else
                    break;
            }
            if (foundCycle == string.Empty)
                Console.WriteLine($"Цикл с длиной {k} не найден.");
            else
                Console.WriteLine($"Найденный цикл с длиной {k} найден: {foundCycle}");
            Console.ReadLine();
            Main();
        }

        static void RandomGraph()
        {
            foundCycle = string.Empty;
            Console.Clear();
            PrintTask();
            int size = Input.ReadInt("Введите количество вершин в графе: ", 3);

            matrix = GenerateMatrix(size);
            visitedNodes = new bool[size];
            PrintMatrix();

            int k = Input.ReadInt("Введите длину цикла, который хотите найти: ", 3, size);
            for (int i = 0; i < size - (k - 1); i++)
            {
                DFS(k - 1, i, i);
                if (!isFound)
                {
                    path.Pop();
                    visitedNodes[i] = true;
                }
                else
                    break;
            }
            if (foundCycle == string.Empty)
                Console.WriteLine($"Цикл с длиной {k} не найден.");
            else
                Console.WriteLine($"Найденный цикл с длиной {k} найден: {foundCycle}");
            Console.ReadLine();
            Main();
        }

        static int[,] GenerateMatrix(int size)
        {
            int?[,] randomMatrix = new int?[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        randomMatrix[i, j] = 0;
                    else
                    {
                        if (randomMatrix[i, j] == null)
                        {
                            randomMatrix[i, j] = rand.Next(0, 2);
                            randomMatrix[j, i] = randomMatrix[i, j];
                        }
                    }
                }
            }

            int[,] normalMatrix = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    normalMatrix[i, j] = randomMatrix[i, j] ?? default(int);

            return normalMatrix;
        }

        static void PrintMatrix()
        {
            Console.WriteLine("\n=================\n" +
                              "Введенная матрица смежности:");
            Console.Write("  ");
            for (int i = 0; i < matrix.GetLength(0); i++)
                Console.Write($"{(char)('A' + i)} ");
            Console.WriteLine();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write($"{(char)('A' + i)} ");
                for (int j = 0; j < matrix.GetLength(0); j++)
                    Console.Write($"{matrix[i, j]} ");
                Console.WriteLine();
            }
        }
    }
}
