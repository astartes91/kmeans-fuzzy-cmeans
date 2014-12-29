using System;
using System.Collections.Generic;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Шаги алгоритма C-Means
    /// </summary>
    public partial class FuzzyCMeansAlgorithm
    {
        /// <summary>
        /// Создание нечётких кластеров
        /// </summary>
        /// <param name="points">координаты точек</param>
        /// <param name="clusterCenters">центры кластеров</param>
        /// <param name="fuzzificationParameter">параметр фаззификации</param>
        /// <param name="membershipMatrix">ссылка на переменную матрицы членства</param>
        /// <returns>отображение "координаты точки" -> "координаты центра кластера"</returns> 
        private static Dictionary<List<double>, List<double>> MakeFuzzyClusters(List<List<double>> points, List<List<double>> clusterCenters,
            double fuzzificationParameter, out Dictionary<List<double>, List<double>> membershipMatrix)
        {
            //расстояния от точек до центров всех кластеров
            Dictionary<List<double>, List<double>> distancesToClusterCenters = AlgorithmsUtils.CalculateDistancesToClusterCenters(points, clusterCenters);
            Dictionary<List<double>, List<double>> clusters = new Dictionary<List<double>, List<double>>();
            //матрица членства - Отображение "координаты точки" -> "значения функции членства точки во всех кластерах"
            membershipMatrix = CreateMembershipMatrix(distancesToClusterCenters, fuzzificationParameter);

            foreach (KeyValuePair<List<double>, List<double>> value in membershipMatrix)
            {
                //получение номера кластера с ближайшим центром
                int clusterNumber = ListUtils.GetMaxIndex(value.Value);
                clusters.Add(value.Key, clusterCenters[clusterNumber]);
            }

            return clusters;
        }

        /// <summary>
        /// Этот метод создаёт матрицу членства
        /// </summary>
        /// <param name="distancesToClusterCenters">расстояния от точек до всех центров кластеров отображение "координаты точки" -> "расстояния до всех 
        /// центров кластеров"</param>
        /// <param name="fuzzificationParameter">параметр фаззификации</param>
        /// <returns>Отображение "координаты точки" -> "значения функции членства точки во всех кластерах"</returns> 
        private static Dictionary<List<double>, List<double>> CreateMembershipMatrix(Dictionary<List<double>, List<double>> distancesToClusterCenters,
            double fuzzificationParameter)
        {
            Dictionary<List<double>, List<double>> map = new Dictionary<List<double>, List<double>>();

            foreach (KeyValuePair<List<double>, List<double>> pair in distancesToClusterCenters)
            {
                List<double> unNormaizedMembershipValues = new List<double>();
                double sum = 0;

                for (int i = 0; i < pair.Value.Count; i++)
                {
                    double distance = pair.Value[i];
                    if (distance == 0)
                    {
                        distance = 0.0000001;
                    }

                    double membershipValue = Math.Pow(1 / distance, (1 / (fuzzificationParameter - 1)));
                    sum += membershipValue;
                    unNormaizedMembershipValues.Add(membershipValue);
                }

                List<double> membershipValues = new List<double>();
                foreach (double membershipValue in unNormaizedMembershipValues)
                {
                    membershipValues.Add((membershipValue / sum));                 
                }
                map.Add(pair.Key, membershipValues);
            }

            return map;
        }

        /// <summary>
        /// Расчёт новых центров кластеров
        /// </summary>
        /// <param name="clusterCenters">текущие координаты центров кластеров</param>
        /// <param name="membershipMatrix">матрица членства - отображение "координаты точки" -> "значения функции членства точки во всех кластерах"</param>
        /// <param name="fuzzificationParameter">параметр фаззификации</param>
        /// <returns>Новые координаты центров кластеров</returns>
        private static List<List<double>> RecalculateCoordinateOfFuzzyClusterCenters(List<List<double>> clusterCenters, 
            Dictionary<List<double>, List<double>> membershipMatrix, double fuzzificationParameter)
        {
            List<double> clusterMembershipValuesSums = new List<double>();
            foreach (List<double> clusterCenter in clusterCenters)
            {
                clusterMembershipValuesSums.Add(0);
            }

            List<List<double>> weightedPointCoordinateSums = new List<List<double>>();
            for (int i = 0; i < clusterCenters.Count; i++)
            {
                List<double> clusterCoordinatesSum = new List<double>();
                foreach (double coordinate in clusterCenters[0])
                {
                    clusterCoordinatesSum.Add(0);
                }

                foreach (KeyValuePair<List<double>, List<double>> pair in membershipMatrix)
                {
                    List<double> pointCoordinates = pair.Key;
                    List<double> membershipValues = pair.Value;

                    clusterMembershipValuesSums[i] += Math.Pow(membershipValues[i], fuzzificationParameter);

                    for (int j = 0; j < pointCoordinates.Count; j++)
                    {
                        clusterCoordinatesSum[j] += pointCoordinates[j] * Math.Pow(membershipValues[i], fuzzificationParameter);
                    }
                }

                weightedPointCoordinateSums.Add(clusterCoordinatesSum);
            }

            List<List<double>> newClusterCenters = new List<List<double>>();
            for (int i = 0; i < clusterCenters.Count; i++)
            {
                List<double> newCoordinates = new List<double>();
                List<double> coordinatesSums = weightedPointCoordinateSums[i];
                for (int j = 0; j < coordinatesSums.Count; j++)
                {
                    newCoordinates.Add(coordinatesSums[j]/clusterMembershipValuesSums[i]);
                }

                newClusterCenters.Add(newCoordinates);
            }

            return newClusterCenters;
        }
    }
}
