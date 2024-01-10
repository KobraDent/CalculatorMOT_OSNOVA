using System;

namespace CalculatorMOT
{
    // Класс, представляющий элемент истории операций калькулятора
    public class HistoryItem
    {
        // Выражение, связанное с операцией
        public string Expression { get; set; }
        // Результат выполненной операции
        public double Result { get; set; }
        // Временная метка, указывающая на момент записи операции в историю.
        public DateTime Timestamp { get; set; }
    }
}
