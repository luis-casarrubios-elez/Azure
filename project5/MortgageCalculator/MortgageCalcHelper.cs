using System;

namespace MortgageCalculator
{
    public class MortgageCalcHelper
    {
        // use the function to calculate the monthly payment   
        public static double ComputeMonthlyPayment(double principal, double years, double rate)
        {
            double monthly = 0;
            double top = principal * rate / 1200.00;
            double bottom = 1 - Math.Pow(1.0 + rate / 1200.0, -12.0 * years);
            // http://www.bankrate.com/calculators/mortgages/loan-calculator.aspx
            monthly = top / bottom;
            //Console.WriteLine();
            //Console.WriteLine("With a principl of ${0}, duration of {1} years and a interest rate of {2}% the monthly loan payment amount is {3:$0.00}", principal, years, rate, monthly);
            return monthly;
        }
    }
}
