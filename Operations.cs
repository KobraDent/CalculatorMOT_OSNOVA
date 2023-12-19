using System;
// Класс Operation представляет отдельную операцию (запись) в истории вычислений
public class Operation
{
    // Идентификатор операции
    public int Id { get; set; }
    // Выражение, связанное с операцией
    public string Expression { get; set; }
    // Результат вычислений
    public double Result { get; set; }
    public DateTime Timestamp { get; set; }
}
