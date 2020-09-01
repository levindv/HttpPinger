using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace HttpPinger
{
    public class TraceVM : INotifyPropertyChanged
    {
        public TraceVM()
        {
            History = new ObservableCollection<HistoryItemVM>();
        }

        #region string Address
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
                }
            }
        }
        private string _address;
        #endregion

        #region int[] SuccessStatuses
        public int[] SuccessStatuses
        {
            get { return _successStatuses; }
            set
            {
                if (_successStatuses != value)
                {
                    _successStatuses = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SuccessStatuses)));
                }
            }
        }
        private int[] _successStatuses;
        #endregion

        #region int[] FailStatuses
        public int[] FailedStatuses
        {
            get { return _failStatuses; }
            set
            {
                if (_failStatuses != value)
                {
                    _failStatuses = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FailedStatuses)));
                }
            }
        }
        private int[] _failStatuses;
        #endregion

        #region ObservableCollection<HistoryItemVM> History
        public ObservableCollection<HistoryItemVM> History
        {
            get { return _history; }
            set
            {
                if (_history != value)
                {
                    _history = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(History)));
                }
            }
        }
        private ObservableCollection<HistoryItemVM> _history;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddEvent(int? statusCode, TimeSpan timeout)
        {
            EStatusType statusType;

            if (!statusCode.HasValue || statusCode.Value <= 0)
            {
                statusType = EStatusType.Error;
            }
            else if (SuccessStatuses.Contains(statusCode.Value))
            {
                statusType = EStatusType.Success;
            }
            else if (FailedStatuses.Contains(statusCode.Value))
            {
                statusType = EStatusType.Error;
            }
            else
            {
                statusType = EStatusType.Warning;
            }

            History.Add(new HistoryItemVM(statusType, statusCode ?? 0, DateTime.Now, timeout));

            while (History.Count > 1000)
            {
                History.RemoveAt(0);
            }
        }
    }
}