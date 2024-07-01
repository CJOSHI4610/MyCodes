using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Net;
using DA.Pricebook.Backend.Helpers;
using DA.Pricebook.Backend.Models;
using System.Text.Json;

namespace DanishAgroTest
{
    public static class GetPriceDatas
    {

        private static PriceDataHelper _db;
        private static PricebookDataFormatter _formatter;
        private static List<Category> _categories;

        [FunctionName("GetPriceDatas")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (_db == null)
            {
                _db = new PriceDataHelper(log);
                _formatter = new PricebookDataFormatter(log);
            }
            try
            {
                var request = JsonSerializer.Deserialize<GetPriceDatasRequest>(await req.ReadAsStringAsync());

                //adding code for handling daily/periodic and regionwise prices
                request = SanitizeRequest(request);

                var result = await _db.GetPriceDatasAsync(request);
                var dailyRegions = new List<string>() { "Daily" };
                var formattedData = await _formatter.GetPricebookDataAsync(result, (request.IsPeriodPrice ? request.Regions : dailyRegions));
                Console.WriteLine(formattedData.ToString());

                return new OkObjectResult(formattedData);

            
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.ToString());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        private static GetPriceDatasRequest SanitizeRequest(GetPriceDatasRequest request)
        {
            if (request.Periods == null)
            {
                request.Periods = new List<Period>();
            }

            if (!request.IsDailyPrice && !request.IsPeriodPrice)
            {
                request.IsPeriodPrice = true;
                //if (!request.IsEast && !request.IsWest)
                //{
                //    request.IsEast = true;
                //    request.IsWest = true;
                //}
            }

            return request;
        }
    }
}
