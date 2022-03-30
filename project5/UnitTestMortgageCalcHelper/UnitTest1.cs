using Microsoft.VisualStudio.TestTools.UnitTesting;
using MortgageCalculator;

namespace UnitTestMortgageCalcHelper
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow(1000000, 15, 5, 7907.9362674154645)]
        [DataRow(550000, 25, 3.5, 2753.429636427204)]
        [DataRow(2500000, 30, 2.3, 9620.032264450625)]
        [DataRow(144000, 8, 8.2, 2050.3501846044037)]
        [DataRow(800000, 10, 4, 8099.611053190365)]
        public void ComputeMonthlyPaymentTest(double principal, double years, double rate, double expected)
        {
            var result = MortgageCalcHelper.ComputeMonthlyPayment(principal,years,rate);
            Assert.AreEqual(expected, result);
        }
    }
}
