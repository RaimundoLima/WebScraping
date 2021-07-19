using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebScraping.Lib.Models;

using PuppeteerSharp;

namespace WebScraping.Lib.Service
{
    public class AnalysisService
    {
        public  AnalysisModel AnalysisUrl(string url)
        {

            AnalysisModel analysisModel = new AnalysisModel();
            new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }).Result;
            try
            {
                var page = browser.NewPageAsync().Result;
                page.GoToAsync(url).Wait();
                LoadContentInfinityScroll(page);


                var textContent = page.EvaluateFunctionAsync<dynamic>("()=>{return document.body.innerText}").Result;
                analysisModel.Words = ContentToWord(textContent);

                string htmlContent = page.GetContentAsync().Result;
                analysisModel.Images = HtmlToImageModel(htmlContent, new Uri(url));
            }
            catch (Exception ex) {
                browser.Dispose();
                throw ex;
            }

            browser.Dispose();
            return analysisModel;

        }
        
        public List<WordRankModel> ContentToWord(string textContent)
        {
            List<WordRankModel> words = new List<WordRankModel>();
            Regex patternArticlesPt = new Regex(@"(?m)(?i)(\Wa |\We |\Wo |\Wao |\Wos |\Wdas |\Wdos |\Waos |\Wde |\Wou )");
            Regex patternArticlesEn = new Regex(@"(?m)(?i)(\Wof |\Won |\Wthe |\Wa |\Wan |\Wand |\Wfor |\Wor |\Wto |\Was |\Win |\Wis |\Ware |\Wby )");
            Regex patternNumbers = new Regex(@"(?m)(?i)[\d*]");
            Regex patternOnlyWorld = new Regex(@"(?m)(?i)[\wÀ-ú]\w*");



            textContent = patternArticlesPt.Replace(textContent, " ");
            textContent = patternArticlesEn.Replace(textContent, " ");
            textContent = patternNumbers.Replace(textContent, " ");

            List<string> listWords = patternOnlyWorld.Matches(textContent).Cast<Match>().Select(c => c.Value.ToLower()).ToList();
            foreach (string word in listWords)
            {
                if (words.Exists(x => x.Value == word))
                {
                    words.Find(x => x.Value == word).Quantity++;
                }
                else
                {
                    if (word.Length > 1)
                    {
                        words.Add(new WordRankModel { Value = word, Quantity = 1 });
                    }
                }
            }

            
            return words.OrderByDescending(x=>x.Quantity).ToList();

        }

        public List<ImageModel> HtmlToImageModel(string htmlContent, Uri uri)
        {
            htmlContent = htmlContent.Replace("u002F", "");
            List<ImageModel> images = new List<ImageModel>();

            Regex patternImageExt = new Regex(@"(?m)(?i)([a-z0-9\/\:\-_\\\.@]*(\.jpg|\.png|\.jpeg|\.gif|\.ico))");
            Regex patternImageSrc = new Regex("(?m)(?i)(alt=\")(?<alt1>.*?)([\"'´].*?)(src=.)(?<src1>.*?)([\"'´].*?)|(src=.)(?<src2>.*?)([\"'´].*?)(alt=\")(?<alt2>.*?)([\"'´].*?)|(src=.)(?<src3>.*?)([\"'´].*?)");

            List<string> listUrl = patternImageExt.Matches(htmlContent).Cast<Match>().Select(c => c.Value).ToList();
            List<Match> tempMatch = patternImageSrc.Matches(htmlContent).Cast<Match>().ToList();
            Dictionary<string, string> altImage = new Dictionary<string, string>();
            foreach (Match match in tempMatch)
            {
                string alt = match.Groups.Values.Where(x => { return (!string.IsNullOrWhiteSpace(x.Value) && x.Name == "alt1") || (!string.IsNullOrWhiteSpace(x.Value) && x.Name == "alt2"); }).FirstOrDefault()?.Value;
                string src = match.Groups.Values.Where(x => { return (!string.IsNullOrWhiteSpace(x.Value) && x.Name == "src1") || (!string.IsNullOrWhiteSpace(x.Value) && x.Name == "src2") || (!string.IsNullOrWhiteSpace(x.Value) && x.Name == "src3"); }).FirstOrDefault()?.Value;
                if (!string.IsNullOrWhiteSpace(alt))
                {
                    altImage.TryAdd(src, alt);
                }

                listUrl.Add(src);
            }
            images = new List<ImageModel>();
            foreach (string urlImage in listUrl)
            {
                string temp = urlImage;
                ImageModel imageModel;
                if (temp != null)
                {
                    if (!images.Exists(x => x.Url == temp))
                    {
                        if (!temp.ToLower().StartsWith("http") && !temp.ToLower().StartsWith("data"))
                        {
                            if (urlImage.StartsWith("/"))
                            {
                                temp = $"{uri.Scheme}://{uri.Host}{urlImage}";
                            }
                            else
                            {
                                temp = $"{uri.AbsoluteUri}/{urlImage}";
                            }
                        }

                        if (altImage.ContainsKey(urlImage))
                        {
                            imageModel = new ImageModel { Url = temp, Alt = altImage.Where(x => { return x.Key == urlImage; }).First().Value };
                        }
                        else
                        {
                           imageModel=new ImageModel { Url = temp };
                        }
                        if (!images.Exists(x => x.Url == imageModel.Url)) {
                            images.Add(imageModel);
                        }

                    }
                }

            }
            return images;
        }

        private void LoadContentInfinityScroll(Page page)
        {
            string jsFunctionForDynamicContent = @"()=>window.scrollTo(0, 99999999)";

            int documentLength = 0;
            while (true)
            {
                var temp = page.GetContentAsync().Result;
                if (documentLength == temp.Length)
                {
                    break;
                }
                else
                {
                    documentLength = temp.Length;
                    page.EvaluateExpressionAsync<int>(jsFunctionForDynamicContent);
                    Thread.Sleep(5000);
                }

            }
        }
    }
}
