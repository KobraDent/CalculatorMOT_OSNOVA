using System;

namespace CalculatorMOT
{
    public class HistoryItem
    {
        public string Expression { get; set; }
        public double Result { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
