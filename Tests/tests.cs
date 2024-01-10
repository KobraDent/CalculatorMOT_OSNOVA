using Calculator_Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// Атрибут TestClass указывает на то, что этот класс содержит набор тестов
[TestClass]
public class CalculatorTests
{
    // Тест на сложение
    [TestMethod]
    public void Calculator_Parse_Addition()
    {
        // Arrange: Подготовка данных для теста
        string input = "2+2";

        // Act: Выполнение действия (вызов парсера)
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert: Проверка результата (ожидаем, что результат равен 4)
        Assert.AreEqual(4, result);
    }

    // Тест на умножение
    [TestMethod]
    public void Calculator_Parse_Multiplication()
    {
        // Arrange
        string input = "3.14*2";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(6.28m, result);
    }

    // Тест на деление
    [TestMethod]
    public void Calculator_Parse_Division()
    {
        // Arrange
        string input = "8/2";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(4, result);
    }

    // Тест на сложное выражение с использованием скобок
    [TestMethod]
    public void Calculator_Parse_ComplexExpression()
    {
        // Arrange
        string input = "2*(3+4)";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(14, result);
    }

    // Тест на вычитание
    [TestMethod]
    public void Calculator_Parse_Subtraction()
    {
        // Arrange
        string input = "5-3";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(2, result);
    }

    // Тест на выражение с использованием скобок
    [TestMethod]
    public void Calculator_Parse_ExpressionWithParentheses()
    {
        // Arrange
        string input = "(2+3)*4";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(20, result);
    }

    // Тест на десятичное умножение
    [TestMethod]
    public void Calculator_Parse_DecimalMultiplication()
    {
        // Arrange
        string input = "1.5*2.5";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(3.75m, result);
    }

    // Тест на десятичное деление
    [TestMethod]
    public void Calculator_Parse_DecimalDivision()
    {
        // Arrange
        string input = "10/4";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(2.5m, result);
    }

    // Тест на десятичное сложение
    [TestMethod]
    public void Calculator_Parse_DecimalAddition()
    {
        // Arrange
        string input = "0.1+0.2";

        // Act
        Parser parser = new Parser(input);
        decimal result = parser.Parse();

        // Assert
        Assert.AreEqual(0.3m, result);
    }

    // Тест на деление на ноль
    [TestMethod]
    public void Calculator_Parse_DivisionByZero()
    {
        // Arrange
        string input = "2/0";

        // Act & Assert: Ожидаем, что при делении на ноль будет исключение InvalidOperationException
        Parser parser = new Parser(input);
        Assert.ThrowsException<InvalidOperationException>(() => parser.Parse());
    }

    // Тест на некорректный ввод
    [TestMethod]
    public void Calculator_Parse_InvalidInput()
    {
        // Arrange
        string input = "abc";

        // Act & Assert: Ожидаем, что при некорректном вводе будет исключение InvalidOperationException
        Parser parser = new Parser(input);
        Assert.ThrowsException<InvalidOperationException>(() => parser.Parse());
    }
}


