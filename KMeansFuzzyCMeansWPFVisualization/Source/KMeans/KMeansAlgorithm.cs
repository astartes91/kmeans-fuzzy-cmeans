using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Wpf;
using PointAnnotation = OxyPlot.Annotations.PointAnnotation;

namespace KMeansFuzzyCMeansWPFVisualization
{
	/// <summary>
	/// Реализация алгоритма кластеризации K-Means
	/// </summary>
	public partial class KMeansAlgorithm
	{
		/// <summary>
		/// Кластеризация K-Means
		/// </summary>
		/// <param name="plot">Модель графика OxyPlot</param>
		/// <param name="label">Label для показа информации о номере итерации</param>
		/// <param name="numberOfClusters">Число кластеров</param>
		public static void DoClusteringByKMeans(PlotView plot, Label label, int numberOfClusters)
		{
			if (plot.Model == null)
			{
				return;
			}

			//координаты точек
			List<List<double>> coordinates = new List<List<double>>();
			//заполнении информации о координатах точек
			foreach (PointAnnotation annotation in plot.Model.Annotations)
			{
				List<double> pointCoordinates = new List<double> {annotation.X, annotation.Y};
				coordinates.Add(pointCoordinates);
			}

			Random random = new Random();
			byte[] bgrColorComponents = new byte[3];
			//цвета кластеров, выбираются случайно
			Dictionary<List<double>, OxyColor> clusterColors = new Dictionary<List<double>, OxyColor>();

			//первоначальная генерация центров кластеров
			List<List<double>> clusterCenters = AlgorithmsUtils.MakeInitialSeeds(coordinates, numberOfClusters);
			foreach (List<double> clusterCenter in clusterCenters)
			{
				foreach (PointAnnotation annotation in plot.Model.Annotations)
				{
					// todo: разобраться со сравнением флоат-пойнт чисел
					if (annotation.X == clusterCenter[0] && annotation.Y == clusterCenter[1])
					{
						random.NextBytes(bgrColorComponents);
						//отметим на графике центры кластеров
						UIUtils.MarkClusterCenter(annotation, OxyColor.FromRgb(bgrColorComponents[0], bgrColorComponents[1], bgrColorComponents[2]));
						clusterColors.Add(clusterCenter, annotation.Fill);

						#region DEBUG
						#if DEBUG
						Console.WriteLine("Inital cluster center x = {0}, y = {1}", annotation.X, annotation.Y);
						#endif  
						#endregion                      
					}
				}
			}

			bool stop = false;
			Dictionary<List<double>, List<double>> clusters = null;

			int iteration = 0;
			
			//цикл продолжается пока меняются координаты центров кластеров
			while (!stop)
			{
				#region DEBUG
				#if DEBUG
				Console.WriteLine("Iteration = {0}", iteration);
				#endif
				#endregion

				label.Content = "Iteration " + iteration;
				//отображение из координат точки в координаты центра кластера
				clusters = MakeClusters(coordinates, clusterCenters);
				foreach (KeyValuePair<List<double>, List<double>> pair in clusters)
				{
					foreach (PointAnnotation annotation in plot.Model.Annotations)
					{
						// todo: разобраться со сравнением флоат-пойнт чисел
						if (annotation.X == pair.Key[0] && annotation.Y == pair.Key[1])
						{
							//закрашиваем точку цветом кластера
							annotation.Fill = clusterColors[pair.Value];
						    annotation.ToolTip = "Cluster " + ListUtils.GetElementIndex(clusterCenters, pair.Value);
						}
					}
				}

				List<List<double>> oldClusterCenters = clusterCenters;
				//пересчёт центров кластеров 
				clusterCenters = RecalculateCoordinateOfClusterCenters(clusters, clusterCenters);
				
				//если координаты центров кластеров не изменились, выходим из цикла
				if (ListUtils.IsListEqualsToAnother(clusterCenters, oldClusterCenters))
				{
					stop = true;
				}
				//если координаты центров кластеров поменялись изменились, пересчитываем кластеры
				else
				{
					List<OxyColor> colorValues = clusterColors.Values.ToList();
					clusterColors.Clear();
					for (int i = 0; i < clusterCenters.Count; i++)
					{
						clusterColors.Add(clusterCenters[i], colorValues[i]);
					}

					//удаление отображения всех центров кластеров
					foreach (PointAnnotation annotation in plot.Model.Annotations)
					{
						annotation.Shape = MarkerType.Circle;
						annotation.Size = 4;
					}

					//проверка на потенциально не существующие центры кластеров
					foreach (List<double> oldClusterCenter in oldClusterCenters)
					{
						bool isClusterCenterDataPoint = false;
						foreach (List<double> coordinate in coordinates)
						{
							// todo: разобраться со сравнением флоат-пойнт чисел
							if (oldClusterCenter[0] == coordinate[0] && oldClusterCenter[1] == coordinate[1])
							{
								#region DEBUG
								#if DEBUG
								Console.WriteLine("ex-center x = {0}, y = {1}", oldClusterCenter[0], oldClusterCenter[1]);
								#endif
								#endregion

								isClusterCenterDataPoint = true;
								break;
							}
						}
						//если центр кластера не является точкой данных
						if (!isClusterCenterDataPoint)
						{
							foreach (PointAnnotation annotation in plot.Model.Annotations)
							{
								// todo: разобраться со сравнением флоат-пойнт чисел
								if (annotation.X == oldClusterCenter[0] && annotation.Y == oldClusterCenter[1])
								{
									#region DEBUG
									#if DEBUG
									Console.WriteLine("remove point with coordinate x = {0}, y = {1}", annotation.X, annotation.Y);
									#endif
									#endregion

									//удаление центра кластера
									plot.Model.Annotations.Remove(annotation);
									break;
								}
							}
						}
					}

					//Отмечаем новые кластеры на графике
					for (int i = 0; i < clusterCenters.Count; i++)
					{
						List<double> clusterCenter = clusterCenters[i];
						bool isExists = false;
						foreach (PointAnnotation annotation in plot.Model.Annotations)
						{
							// todo: разобраться со сравнением флоат-пойнт чисел
							if (annotation.X == clusterCenter[0] && annotation.Y == clusterCenter[1])
							{
								//если центр кластера с такими координатами существует, помечаем его на графике как центр кластера
								UIUtils.MarkClusterCenter(annotation, colorValues[i]);
								isExists = true;
								break;
							}
						}
						//если центр кластера с такими координатами не существует, создаём на графике новую точку и помечаем её как центр кластера
						if (!isExists)
						{
							PointAnnotation pointAnnotation = new PointAnnotation {X = clusterCenter[0], Y = clusterCenter[1]};
							UIUtils.MarkClusterCenter(pointAnnotation, colorValues[i]);
							plot.Model.Annotations.Add(pointAnnotation);

							#region DEBUG
							#if DEBUG
							Console.WriteLine("add center with coordinate x = {0}, y = {1}", pointAnnotation.X, pointAnnotation.Y);
							#endif
							#endregion
						}
					}
				}

				plot.InvalidatePlot();
				//Thread.Sleep(1000);
				iteration++;
			}
		}
	}
}