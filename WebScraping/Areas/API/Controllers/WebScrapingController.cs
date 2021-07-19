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
using System.Net;
using WebScraping.Areas.API.Model;

namespace WebScraping.Api.Controllers
{
    [ApiController]
    public class WebScrapingController : Controller
    {
        private readonly AnalysisService service = new AnalysisService();


        [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "Url" })]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AnalysisModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [HttpGet("Api/WebScraping")]
        public IActionResult WebScraping([FromQuery] ApiUrlModel model)
        {
            try
            {
                AnalysisModel response = new AnalysisModel();
                if (ModelState.IsValid)
                {
                    response = service.AnalysisUrl(model.Url);
                    if (response.Equals(new AnalysisModel()))
                    {
                        return StatusCode((int)HttpStatusCode.NoContent);
                    }
                    else {
                        return Ok(response);
                    }
                }
                else 
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
