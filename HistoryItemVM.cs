using System;
using System.Windows.Media;

namespace HttpPinger
{
    public class HistoryItemVM
    {
        public HistoryItemVM(EStatusType statusType, int statusCode, DateTime dt, TimeSpan timeout)
        {
            StatusCode = statusCode;
            StatusType = statusType;
            Timestamp = dt;
            Timeout = timeout;
            StatusColor = GetColor(statusType);
        }

        private Brush GetColor(EStatusType statusType)
        {
            var color = statusType switch
            {
                EStatusType.Error => Colors.DarkRed,
                EStatusType.Success => Colors.DarkGreen,
                EStatusType.Warning => Colors.Gold,
                _ => Colors.White,
            };

            return new SolidColorBrush(color);
        }

        public EStatusType StatusType { get; }
        public int StatusCode { get; }
        public DateTime Timestamp { get; }
        public TimeSpan Timeout { get; }
        public Brush StatusColor { get; }
    }
}