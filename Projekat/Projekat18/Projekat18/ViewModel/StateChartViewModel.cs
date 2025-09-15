using Projekat18.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Collections.Generic;

namespace Projekat18.ViewModel
{
    public class StateChartViewModel : INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public StateChartViewModel(ObservableCollection<Database> databases)
        {
            // Grupisanje baza po State.Name i boji
            var stateGroups = databases
                .GroupBy(db => db.State.Name)
                .Select(g => new
                {
                    State = g.First().State.Name,
                    Count = g.Count(),
                    Color = g.First().State.Color ?? "#2196F3"
                })
                .ToList();

            
            SeriesCollection = new SeriesCollection();
            foreach (var group in stateGroups)
            {
                var brush = TryGetBrush(group.Color);
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = group.State, // SVI stubovi imaju isti naziv!
                    Values = new ChartValues<int> { group.Count },
                    Fill = brush,
                    ColumnPadding = 15,
                    MaxColumnWidth = 50
                });
            }
        }

        private SolidColorBrush TryGetBrush(string color)
        {
            try { return (SolidColorBrush)(new BrushConverter().ConvertFrom(color)); }
            catch { return Brushes.Gray; }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}