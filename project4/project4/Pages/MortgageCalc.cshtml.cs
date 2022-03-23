using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace project3
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
                // New HttpClient to use backend API
                HttpClient httpClient = new HttpClient();
                // URL obtained from the CoreWebaPI Properties > launchSettings.json
                var result = await httpClient.GetAsync($"http://localhost:48764/api/MortgageCalc?principal={InputLoan.Value.ToString()}&rate={Interest.Value.ToString()}&years={Duration.Value.ToString()}");
                string resultAsString = await result.Content.ReadAsStringAsync();
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
