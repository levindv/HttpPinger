using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HttpPinger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Timer _timer;
        private readonly HttpClient _client;
        private readonly string settsPath = @"c:\pinger\setts.json";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            if (File.Exists(settsPath))
            {
                var trs = JsonConvert.DeserializeObject<TraceVM[]>(File.ReadAllText(settsPath));
                if (trs != null)
                {
                    Traces = new ObservableCollection<TraceVM>(trs);
                }
            }
            if (Traces == null)
            {
                Traces = new ObservableCollection<TraceVM>();
            }
            _timer = new Timer(DoPing, null, 1000, 1000);
            _client = new HttpClient();
        }

        private void DoPing(object state)
        {
            TraceVM[] localTraces = null;
            Dispatcher.Invoke(() => localTraces = Traces?.ToArray());
            if (localTraces != null)
            {
                Parallel.ForEach(localTraces, t => ThreadPool.QueueUserWorkItem(HttpPing, t));
            }
        }

        private void HttpPing(object state)
        {
            var trace = state as TraceVM;
            var sw = Stopwatch.StartNew();
            try
            {
                var response = _client.GetAsync(trace.Address).Result;
                sw.Stop();
                Dispatcher.Invoke(() => trace.AddEvent((int?)response?.StatusCode, sw.Elapsed));
            }
            catch
            {
                sw.Stop();
                Dispatcher.Invoke(() => trace.AddEvent(-1, sw.Elapsed));
            }
        }

        public ObservableCollection<TraceVM> Traces { get; set; }

        public TraceVM CurrentTrace { get; set; }

        private void AddAddress_Click(object sender, RoutedEventArgs e)
        {
            var trace = new TraceVM();
            var window = new AddressEdittingWindow(trace);
            window.ShowDialog();
            if (!string.IsNullOrWhiteSpace(trace.Address))
            {
                Traces.Add(trace);
            }

            File.WriteAllText(settsPath, JsonConvert.SerializeObject(Traces));
        }

        private void RemoveAddress_Click(object sender, RoutedEventArgs e)
        {
            Traces.Remove(CurrentTrace);
            File.WriteAllText(settsPath, JsonConvert.SerializeObject(Traces));
        }

        private void DataGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // select row
        }
    }
}