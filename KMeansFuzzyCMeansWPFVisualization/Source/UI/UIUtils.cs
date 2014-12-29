using OxyPlot;
using OxyPlot.Annotations;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Методы для работы с графиком на визуальной форме
    /// </summary>
    public class UIUtils
    {
        /// <summary>
        /// Этот метод помечает центры кластеров на графике
        /// </summary>
        /// <param name="clusterCenterPoint">точка центра кластера на графике</param>
        /// <param name="color">Цвет кластера</param>
        public static void MarkClusterCenter(PointAnnotation clusterCenterPoint, OxyColor color)
        {
            clusterCenterPoint.Fill = color;
            clusterCenterPoint.Shape = MarkerType.Diamond;
            clusterCenterPoint.Size = 7;
        }
    }
}