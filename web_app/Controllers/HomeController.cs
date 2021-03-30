using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using web_app.Models;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace web_app.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Read JSON file and deserialize it to object
            string fileName = "json.json";
            string filePath = Server.MapPath("JSONFile");
            JsonDataModel videoDataModel = new JsonDataModel();
            try
            {
                // read json and build object
                if (System.IO.File.Exists(Path.Combine(filePath, fileName)))
                {
                    // read JSON directly from a file
                    using (StreamReader file = System.IO.File.OpenText(Path.Combine(filePath, fileName)))
                    {
                        string fileContent = file.ReadToEnd();
                        videoDataModel = JsonConvert.DeserializeObject<JsonDataModel>(fileContent);
                    }
                }

                // write to csv file using csvhelper 

                // 1. Extract all images with id and imageURL
                using (var writer = new StreamWriter(System.IO.Path.Combine(filePath, "ImageContent.csv")))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {


                        var imagerecords = videoDataModel.data.Select(x => new ImageDataModel() { Id = x.id, imageUrl = x.contentImageUrl }).ToList();
                        csv.WriteRecords<ImageDataModel>(imagerecords);
                    }
                }
                // 2. Extract Video details like ID, Title, Link, Highest video link

                using (var writer = new StreamWriter(System.IO.Path.Combine(filePath, "VideoContent.csv")))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        var videorecords = videoDataModel.data.Select(x => new VideoDetailDataModel() { VideoId = x.id, Title = x.contentTitle, Link = x.url, HighestResolutionVideoUrl = x.ampPreviewUrl }).ToList();
                        csv.WriteRecords<VideoDetailDataModel>(videorecords);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return View();
        }
    }
}
