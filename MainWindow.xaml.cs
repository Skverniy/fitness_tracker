using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace fitness_tracker
{
    public class MonthDataPoint
    {
        private string[] months = { "Янв", "Фев", "Март", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Нояб", "Дек" };

        public int MonthIndex { get; set; }
        public int Steps { get; set; }

        public MonthDataPoint(int monthIndex, int steps)
        {
            MonthIndex = monthIndex;
            Steps = steps;
        }

        public override string ToString()
        {
            if (MonthIndex >= 0 && MonthIndex < months.Length)
            {
                return $"{months[MonthIndex]}: {Steps} шагов";
            }
            return $"{Steps} шагов";
        }
    }

    public partial class MainWindow : Window
    {
        private string[] months = { "Янв", "Фев", "Март", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Нояб", "Дек" };
        private readonly Random random = new();
        private int[] stepsData = Array.Empty<int>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            stepsData = CreateRandomValues(12, 10, 150);
            MonthlySeries.Values = new ChartValues<double>(stepsData.Select(step => (double)step));
            WalkSeries.Values = new ChartValues<double>(CreateRandomValues(8, 10, 120).Select(step => (double)step));
        }

        private int[] CreateRandomValues(int count, int minimum, int maximum)
        {
            return Enumerable.Range(0, count)
                .Select(_ => random.Next(minimum, maximum + 1))
                .ToArray();
        }

        private static System.Collections.Generic.List<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
        {
            var list = new System.Collections.Generic.List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }
                    list.AddRange(FindVisualChildren<T>(child));
                }
            }
            return list;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void TextInfo_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MonthlyChart_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(MonthlyChart);
            var chartWidth = MonthlyChart.ActualWidth;

            if (chartWidth <= 0 || MonthlyTooltipText == null)
            {
                return;
            }

            var monthIndex = (int)Math.Round(position.X / chartWidth * (months.Length - 1));
            monthIndex = Math.Max(0, Math.Min(months.Length - 1, monthIndex));
            MonthlyTooltipText.Text = months[monthIndex];
        }

    }
}

