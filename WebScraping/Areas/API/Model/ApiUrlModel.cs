using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraping.Areas.API.Model
{
    public class ApiUrlModel
    {
        [BindRequired, Url]
        public string Url { get; set; }

    }
}
