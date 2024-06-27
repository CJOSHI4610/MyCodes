using DA.Pricebook.Backend.Helpers;
using DA.Pricebook.Backend.Models.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace DanishAgroTest
{
    public static class GetCategories
    {

        private static CategoryHelper _db;
        private static List<Category> _categories;

        [FunctionName("GetCategories")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequest req,
            ILogger log)
        {
            if (_db == null)
            {
                _db = new CategoryHelper(log);
            }
            try
            {
                if (_categories == null)
                {
                    _categories = await _db.GetCategoryHierarchyAsync();
                }
                return new OkObjectResult(_categories);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.ToString());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
