using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScraping.Lib.Models;

namespace WebScraping.Models
{
    public class JsonResponseModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
