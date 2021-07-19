using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebScraping.Lib.Models;

namespace WebScraping.Models
{
    public class UrlModel
    {
        public string Url { get; set; }
        public AnalysisModel AnalysisModel { get; set; }
    }
}
