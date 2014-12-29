using System;
using System.Collections.Generic;
using System.Linq;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Общие шаги алгоритма
    /// </summary>
    public class AlgorithmsUtils
    {
        /// <summary>
        /// Этот метод подсчитывает евклидово расстояние от точки до всех центров кластеров
        /// </summary>
        /// <param name="points">координаты точек</param>
        /// <param name="clusterCenters">координаты центров кластеров</param>
        /// <returns>Отображение "координаты точки" -> "расстояния до всех центров кластеров"</returns> 
        public static Dictionary<List<double>, List<double>> CalculateDistancesToClusterCenters(List<List<double>> points, List<List<double>> clusterCenters)
        {
            Dictionary<List<double>, List<double>> map = new Dictionary<List<double>, List<double>>();

            foreach (List<double> pointCoordinates in points)
            {
                List<double> distancesToCenters = new List<double>();
                foreach (List<double> clusterCenter in clusterCenters)
                {
                    //расчёт евклидового расстояния
                    double distance = 0;
                    for (int i = 0; i < pointCoordinates.Count; i++)
                    {
                        distance += Math.Pow(pointCoordinates[i] - clusterCenter[i], 2);
                    }
                    distance = Math.Sqrt(distance);
                    distancesToCenters.Add(distance);
                }
                map.Add(pointCoordinates, distancesToCenters);
            }

            return map;
        }

        /// <summary>
        /// Генерирует случайные точки
        /// </summary>
        /// <param name="dimension">Размерность данных</param>
        /// <returns>Координаты точек</returns> 
        public static HashSet<List<double>> GenerateDataPoints(int dimension)
        {
            HashSet<List<double>> coordinates = new HashSet<List<double>>(new ListOfDoubleEqualityComparer());
            Random random = new Random();

            while (coordinates.Count < 50)
            {
                List<double> list = new List<double>();
                for (int j = 0; j < dimension; j++)
                {
                    list.Add(random.Next(0, 100));
                }

                bool b = coordinates.Add(list);

                #region DEBUG
#if DEBUG
                if (!b)
                {
                    Console.WriteLine("Duplicate detected while generating data points");
                }
#endif
                #endregion
            }

            return coordinates;
        }

        /// <summary>
        /// Первоначальная случайная генерация центров кластеров
        /// </summary>
        /// <param name="coordinates">координаты точек</param>
        /// <param name="numberOfClusters">количество кластеров</param>
        /// <returns>координаты центров кластеров</returns>
        public static List<List<double>> MakeInitialSeeds(List<List<double>> coordinates, int numberOfClusters)
        {
            Random random = new Random();
            List<List<double>> coordinatesCopy = coordinates.ToList();
            List<List<double>> initialClusterCenters = new List<List<double>>();
            for (int i = 0; i < numberOfClusters; i++)
            {
                int clusterCenterPointNumber = random.Next(0, coordinatesCopy.Count);
                initialClusterCenters.Add(coordinatesCopy[clusterCenterPointNumber]);
                coordinatesCopy.RemoveAt(clusterCenterPointNumber);
            }

            return initialClusterCenters;
        }
    }
}
