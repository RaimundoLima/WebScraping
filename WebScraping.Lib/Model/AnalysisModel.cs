using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraping.Lib.Models
{
    public class AnalysisModel
    {

        public List<ImageModel> Images { get; set; }
        public List<WordRankModel> Words { get; set; }
    }
    public class WordRankModel
    {
        public string Value { get; set; }
        public int Quantity { get; set; }
    }
    public class ImageModel
    {
        public string Url { get; set; }
        public string Alt { get; set; }

    }
}
