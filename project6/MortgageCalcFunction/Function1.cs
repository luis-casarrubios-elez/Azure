using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using MortgageHelperNetCore31;

namespace MortgageCalcFunction
{
    public static class Function1
    {
        [FunctionName("MonthlyPayment")]
        [OpenApiOperation(operationId: "MonthlyPayment", tags: new[] { "MonthlyPayment" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "Principal", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Loan amount")]
        [OpenApiParameter(name: "Rate", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Interest rate")]
        [OpenApiParameter(name: "Years", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Duration in years")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static IActionResult MonthlyPayment(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = "";
            double monthlyPayment = 0;

            try 
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string prin = req.Query["Principal"];
                string rate = req.Query["Rate"];
                string years = req.Query["Years"];

                if (string.IsNullOrEmpty(prin) ||
                    string.IsNullOrEmpty(rate) ||
                    string.IsNullOrEmpty(years))
                {
                    responseMessage = "This HTTP triggered function needs all the calculator arguments (Principal, rate, years).";
                }
                else
                {
                    monthlyPayment = MortgageCalcHelper.ComputeMonthlyPayment(System.Convert.ToDouble(prin), System.Convert.ToDouble(years), System.Convert.ToDouble(rate));
                    var roundedMonthly = System.Math.Round(monthlyPayment, 2);
                    responseMessage = $"The monthly payment is ${roundedMonthly} for a loan of ${prin} for {years} years and an interest rate of {rate}%";
                }
            }
            catch (Exception ex)
            {
                responseMessage = ex.Message;
                Console.Error.WriteLine(ex.Message + " " + ex.StackTrace);
            }
            return new OkObjectResult(responseMessage);
        }
    }
}

