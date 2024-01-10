using Calculator_Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class CalculatorTests2
{
    [TestMethod]
    public void TestExpression()
    {
        // Тестирование различных выражений

        // Вызываем метод TestExpression для каждого выражения и ожидаемого результата
        TestExpression("2+2", 4);
        TestExpression("3.14*2", 6.28m);
        TestExpression("8/2", 4);
        TestExpression("2*(3+4)", 14);
        TestExpression("5-3", 2);
        TestExpression("(2+3)*4", 20);
        TestExpression("1.5*2.5", 3.75m);
        TestExpression("10/4", 2.5m);
        TestExpression("0.1+0.2", 0.3m);
    }

    private void TestExpression(string input, decimal expected)
    {
        // Метод для тестирования конкретного выражения

        // Arrange: Создаем объект парсера с заданным выражением
        Parser parser = new Parser(input);

        // Act: Выполняем парсинг выражения
        decimal result = parser.Parse();

        // Assert: Проверяем, соответствует ли результат ожидаемому значению
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestDivisionByZero()
    {
        // Тестирование деления на ноль

        // Arrange, Act, Assert

        // Arrange: Нет необходимости, так как метод TestExpression обрабатывает все аспекты подготовки данных
        // Act & Assert: Ожидаем, что при делении на ноль будет исключение InvalidOperationException
        Assert.ThrowsException<InvalidOperationException>(() => TestExpression("2/0", 0));
    }

    [TestMethod]
    public void TestInvalidInput()
    {
        // Тестирование некорректного ввода

        // Arrange: Нет необходимости, так как метод TestExpression обрабатывает все аспекты подготовки данных
        // Act & Assert: Ожидаем, что при некорректном вводе исключение InvalidOperationException
        Assert.ThrowsException<InvalidOperationException>(() => TestExpression("abc", 0));
    }
}

