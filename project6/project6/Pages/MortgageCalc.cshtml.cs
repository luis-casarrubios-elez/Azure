using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace project6
{
    public class MortgageCalcModel : PageModel
    {
        public string MortgageResult { get; set; }

        [BindProperty]
        [Required]
        public double? InputLoan { get; set; }
        [BindProperty]
        [Required]
        public double? Interest { get; set; }
        [BindProperty]
        [Required]
        public double? Duration { get; set; }

        public async Task OnPost()
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                var functionResult = await httpClient.GetAsync($"http://localhost:7071/api/MonthlyPayment?Principal={InputLoan.Value.ToString()}&Rate={Interest.Value.ToString()}&Years={Duration.Value.ToString()}");
                string resultAsString = await functionResult.Content.ReadAsStringAsync();
                double.TryParse(resultAsString, out double unroundedMonthly);
                var Monthly = System.Math.Round(unroundedMonthly, 2);

                MortgageResult = $"The monthly payment is ${Monthly} for a loan of ${InputLoan.Value} for {Duration.Value} years and an interest rate of {Interest.Value}%";
            }
            else
            {
                if (String.IsNullOrEmpty(ModelState["InputLoan"].AttemptedValue) 
                    || String.IsNullOrEmpty(ModelState["Duration"].AttemptedValue) 
                    || String.IsNullOrEmpty(ModelState["Interest"].AttemptedValue)
                    || String.IsNullOrWhiteSpace(ModelState["InputLoan"].AttemptedValue) 
                    || String.IsNullOrWhiteSpace(ModelState["Duration"].AttemptedValue) 
                    || String.IsNullOrWhiteSpace(ModelState["Interest"].AttemptedValue))
                {
                    MortgageResult = "Blanks are not valid.";
                }
                else
                {
                    MortgageResult = "You have entered not valid parameters, try again.";
                }
            }
        }
    }
}
