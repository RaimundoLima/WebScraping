using System;
using System.Collections.Generic;
using System.Linq;
using WebScraping.Lib.Models;
using WebScraping.Lib.Service;
using Xunit;
namespace WebScraping.Test.Unit
{
    public class AnalysisServiceUnitTest
    {
        [Fact]
        [Trait("Category", "Unit")]
        public void Count_How_Many_Distinct_Words_Text_Have()
        {
            // Arrenge
            string dumbText = "One one two two two two three four five six sevne eight nine teen";
            int expectResult = 10;
            AnalysisService analysisService = new AnalysisService();

            // Act

            var result = analysisService.ContentToWord(dumbText).Count;

            // Assert
            Assert.Equal(expectResult, result);
        }
        [Fact]
        [Trait("Category", "Unit")]
        public void Rank_Words_By_Quantity()
        {
            // Arrenge
            string dumbText = "One One One Two Two Three ";
            var expectResult = new List<WordRankModel>() ;
            expectResult.Add(new WordRankModel{ Value = "one",Quantity = 3 });;
            expectResult.Add(new WordRankModel{Value= "two",Quantity = 2});
            expectResult.Add(new WordRankModel{Value= "three",Quantity = 1});
            AnalysisService analysisService = new AnalysisService();

            // Act

            var result = analysisService.ContentToWord(dumbText);

            // Assert
            Assert.True(expectResult.First().Quantity == result.First().Quantity && expectResult.Last().Value == expectResult.Last().Value  ) ;
        }
        [Fact]
        [Trait("Category", "Unit")]
        public void Count_How_Many_Distinct_Images_Html_Have()
        {
            // Arrenge
            string dumbHtml = @"
            <head>
            </head>
            <body>
                <span>LoremIpsum</span>
                <img src='img1.jpg'/>
                <img src='img2.jpg'/>
                <img src='img2.jpg'/>
                <img src='img3.png'/>
            </body>
            ";
            var expectResult =3;
            var dumbUrl = new Uri("http://www.dumpUrl.com");
            AnalysisService analysisService = new AnalysisService();

            // Act

            var result = analysisService.HtmlToImageModel(dumbHtml, dumbUrl).Count ;

            // Assert
            Assert.Equal(expectResult, result);
        }


    }
}
