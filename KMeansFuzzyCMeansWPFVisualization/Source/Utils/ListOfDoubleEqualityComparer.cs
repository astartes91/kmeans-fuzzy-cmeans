using System.Collections.Generic;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Сравнение List 'double'
    /// </summary>
    public class ListOfDoubleEqualityComparer : IEqualityComparer<List<double>>
    {
        /// <summary>
        /// Переопределение метода Equals для List 'double'
        /// </summary>
        /// <param name="x">Первый лист</param>
        /// <param name="y">Второй лист</param>
        /// <returns></returns>
        public bool Equals(List<double> x, List<double> y)
        {
            return ListUtils.IsListEqualsToAnother(x, y);
        }

        /// <summary>
        /// Получение хэш-кода для List 'double'
        /// </summary>
        /// <param name="obj">объект хэширования</param>
        /// <returns></returns>
        public int GetHashCode(List<double> obj)
        {
            int sum = 0;
            obj.ForEach(i => sum += i.GetHashCode());
            return sum;
        }
    }
}