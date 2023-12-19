using Calculator_Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
// Атрибут TestClass указывает на то, что класс содержит тесты
[TestClass]
public class CalculatorTests
{
    [TestMethod]
    // Тест на сложение
    public void Calculator_Parse_Addition()
    {
        // Arrange
        string input = "2+2";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(4, result);
    }

    [TestMethod]
    // Тест на умножение
    public void Calculator_Parse_Multiplication()
    {
        // Arrange
        string input = "3.14*2";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(6.28, result);
    }

    [TestMethod]
    // Тест на деление
    public void Calculator_Parse_Division()
    {
        // Arrange
        string input = "8/2";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(4, result);
    }

    [TestMethod]
    // Тест на сложное выражение с скобками
    public void Calculator_Parse_ComplexExpression()
    {
        // Arrange
        string input = "2*(3+4)";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(14, result);
    }

    [TestMethod]
    // Тест на вычитание
    public void Calculator_Parse_Subtraction()
    {
        // Arrange
        string input = "5-3";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void Calculator_Parse_ExpressionWithParentheses()
    {
        // Arrange
        string input = "(2+3)*4";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(20, result);
    }

    [TestMethod]
    // Тест на десятичное умножение
    public void Calculator_Parse_DecimalMultiplication()
    {
        // Arrange
        string input = "1.5*2.5";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(3.75, result);
    }

    [TestMethod]
    // Тест на десятичное деление
    public void Calculator_Parse_DecimalDivision()
    {
        // Arrange
        string input = "10/4";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(2.5, result);
    }

    [TestMethod]
    // Тест на десятичное сложение
    public void Calculator_Parse_DecimalAddition()
    {
        // Arrange
        string input = "0.1+0.2";
        Parser parser = new Parser(input);

        // Act
        double result = parser.Parse();

        // Assert
        Assert.AreEqual(0.3, result, 0.0001);
    }

    [TestMethod]
    // Тест на деление на ноль
    public void Calculator_Parse_DivisionByZero()
    {
        // Arrange
        string input = "2/0";
        Parser parser = new Parser(input);

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => parser.Parse());
    }

    [TestMethod]
    // Тест на некорректный ввод
    public void Calculator_Parse_InvalidInput()
    {
        // Arrange
        string input = "abc";
        Parser parser = new Parser(input);

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => parser.Parse());
    }
}
