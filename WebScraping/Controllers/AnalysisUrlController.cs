using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebScraping.Models;
using WebScraping.Lib.Service;
using WebScraping.Lib.Models;

namespace WebScraping.Controllers
{
    public class AnalysisUrlController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<AnalysisUrlController> _logger;
        private readonly AnalysisService service = new AnalysisService();

        public AnalysisUrlController(ILogger<AnalysisUrlController> logger)
        {
            _logger = logger;
        }
        [ResponseCache(Duration = 60)]
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "Url" })]
        [HttpGet]
        public IActionResult AnalysisUrl(UrlModel model)
        {
            JsonResponseModel<UrlModel> response = new JsonResponseModel<UrlModel>();
            try
            {

                response.Data = model;
                response.Success = true;

                response.Data.AnalysisModel = service.AnalysisUrl(model.Url);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
            }
            return Json(response);
        }
    }
}
