using System;
using System.Collections.Generic;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Класс утилит для работы с листами
    /// </summary>
    public class ListUtils
    {
        /// <summary>
        /// Сравнивает листы поэлементно
        /// </summary>
        /// <param name="list1">Первый аргумент для сравнения</param>
        /// <param name="list2">Второй аргумент для сравнения</param>
        /// <returns>Результат поэлементного сравнения листов</returns>
        public static bool IsListEqualsToAnother(List<double> list1, List<double> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                // todo: разобраться со сравнением флоат-пойнт чисел                
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Сравнивает листы поэлементно
        /// </summary>
        /// <param name="list1">Первый аргумент для сравнения</param>
        /// <param name="list2">Второй аргумент для сравнения</param>
        /// <returns>Результат поэлементного сравнения листов</returns>
        public static bool IsListEqualsToAnother(List<List<double>> list1, List<List<double>> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].Count != list2[i].Count)
                {
                    return false;
                }
            }

            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list1[i].Count; j++)
                {
                    // todo: разобраться со сравнением флоат-пойнт чисел
                    if (list1[i][j] != list2[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Получение индекса элемента с минимальным значением в листе
        /// </summary>
        /// <param name="values">лист значений</param>
        /// <returns>индекс элемента с минимальным значением в листе</returns> 
        public static int GetMinIndex(List<double> values)
        {
            double min = double.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }

        /// <summary>
        /// Получение индекса элемента с максимальным значением в листе
        /// </summary>
        /// <param name="values">лист значений</param>
        /// <returns>индекс элемента с максимальным значением в листе</returns> 
        public static int GetMaxIndex(List<double> values)
        {
            double max = double.MinValue;
            int maxIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        /// <summary>
        /// Получение элемента с максимальным значением в листе
        /// </summary>
        /// <param name="values">лист значений</param>
        /// <returns>элемент с максимальным значением в листе</returns> 
        public static double GetMaxElement(List<List<double>> values)
        {
            double max = double.MinValue;
            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values[0].Count; j++)
                {
                    if (values[i][j] > max)
                    {
                        max = values[i][j];
                    }
                }
                
            }

            return max;
        }

        /// <summary>
        /// Создание матрицы различий между двумя матрицами
        /// </summary>
        /// <param name="matrix1">Уменьшаемая матрица</param>
        /// <param name="matrix2">Вычитаемая матрица</param>
        /// <returns>Матрица-разность</returns>
        public static List<List<double>> CreateDifferencesMatrix(List<List<double>> matrix1, List<List<double>> matrix2)
        {
            List<List<double>> differences = new List<List<double>>();
            for (int i = 0; i < matrix1.Count; i++)
            {
                List<double> rowDifferences = new List<double>();
                for (int j = 0; j < matrix1[0].Count; j++)
                {
                    double result = Math.Abs(matrix1[i][j] - matrix2[i][j]);
                    rowDifferences.Add(result);
                }

                differences.Add(rowDifferences);
            }

            return differences;
        }


        /// <summary>
        ///Получение индекса указанного элемента
        /// </summary>
        /// <param name="list">Лист</param>
        /// <param name="element">Элемент</param>
        /// <returns>Индекс указанного элемента</returns>
        public static int GetElementIndex(List<List<double>> list, List<double> element)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (IsListEqualsToAnother(list[i], element))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}