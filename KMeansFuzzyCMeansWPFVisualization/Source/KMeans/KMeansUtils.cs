using System.Collections.Generic;
using System.Linq;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Шаги алгоритма K-Means
    /// </summary>
    public partial class KMeansAlgorithm
    {
        /// <summary>
        /// Расчёт новых центров кластеров как среднего арфиметического координат точек, которые им принадлежат
        /// </summary>
        /// <param name="clusters">кластеры как отображение "координаты точки" -> "координаты центра кластера"</param>
        /// <param name="clusterCenters">текущие координаты центров кластеров</param>
        /// <returns>Новые координаты центров кластеров</returns>
        private static List<List<double>> RecalculateCoordinateOfClusterCenters(Dictionary<List<double>, List<double>> clusters, List<List<double>> clusterCenters)
        {
            List<List<double>> newClusterCenters = new List<List<double>>();
            foreach (List<double> clusterCenter in clusterCenters)
            {
                // возвращает только те точки, которые принадлежат кластеру с центром в clusterCenter
                var map = clusters.Where(point => ListUtils.IsListEqualsToAnother(point.Value, clusterCenter));

                List<double> sums = new List<double>();
                for (int i = 0; i < clusterCenter.Count; i++)
                {
                    sums.Add(0);
                }

                foreach (KeyValuePair<List<double>, List<double>> point in map)
                {
                    List<double> pointCoordinates = point.Key;
                    for (int i = 0; i < pointCoordinates.Count; i++)
                    {
                        sums[i] += pointCoordinates[i];
                    }
                }

                for (int i = 0; i < sums.Count; i++)
                {
                    sums[i] /= map.Count();
                }

                newClusterCenters.Add(sums);
            }

            return newClusterCenters;
        }

        /// <summary>
        /// Создание кластеров
        /// </summary>
        /// <param name="points">координаты точек</param>
        /// <param name="clusterCenters">центры кластеров</param>
        /// <returns>отображение "координаты точки" -> "координаты центра кластера"</returns> 
        private static Dictionary<List<double>, List<double>> MakeClusters(List<List<double>> points, List<List<double>> clusterCenters)
        {
            //расстояния от точек до центров всех кластеров
            Dictionary<List<double>, List<double>> distancesToClusterCenters = AlgorithmsUtils.CalculateDistancesToClusterCenters(points, clusterCenters);
            Dictionary<List<double>, List<double>> clusters = new Dictionary<List<double>, List<double>>();

            foreach (KeyValuePair<List<double>, List<double>> distanceToClusterCenter in distancesToClusterCenters)
            {
                //получение номера кластера с ближайшим центром
                int clusterNumber = ListUtils.GetMinIndex(distanceToClusterCenter.Value);
                clusters.Add(distanceToClusterCenter.Key, clusterCenters[clusterNumber]);
            }

            return clusters;
        }
    }
}
