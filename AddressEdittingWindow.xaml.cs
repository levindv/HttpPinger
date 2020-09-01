using System.Linq;
using System.Windows;

namespace HttpPinger
{
    /// <summary>
    /// Interaction logic for AddressEdittingWindow.xaml
    /// </summary>
    public partial class AddressEdittingWindow : Window
    {
        private readonly TraceVM _traceVM;

        public AddressEdittingWindow(TraceVM traceVM)
        {
            InitializeComponent();

            _traceVM = traceVM;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _traceVM.Address = AddressTB.Text;
            _traceVM.FailedStatuses = GetIntArray(FailedCodesTB.Text);
            _traceVM.SuccessStatuses = GetIntArray(SuccessCodesTB.Text);
            this.Close();
        }

        private int[] GetIntArray(string text)
        {
            var items = text.Split(',', '.', ';', ':', '/', '\\', '|', '*');

            return items.Select(i => i.Trim()).Select(i => int.TryParse(i, out var res) ? res : -1).ToArray();
        }
    }
}