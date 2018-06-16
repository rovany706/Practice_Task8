using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputHelper;

namespace Task8
{
    class Program
    {
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
        static void Main(string[] args)
        {
            Console.WriteLine("Задача 8\n=================");
            Console.WriteLine("Условие задачи:\nГраф задан матрицей смежности.\n" +
                              "Найти в нем какой-либо простой цикл из K вершин.\n" +
                              "=================");

            int n = Input.ReadInt("Введите количество вершин в графе: ", 1);
            int[,] matrix = new int[n, n];
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

            Console.WriteLine("\n=================\n" +
                              "Введенная матрица смежности:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
