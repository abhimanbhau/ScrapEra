using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
using ScrapEra.ScrapLogger;

namespace ScrapEra.Utils
{
    public class PageLanguageDetection
    {
        public static string GetContentLanguage(string content)
        {
            try
            {
                var client = new RestClient("http://ws.detectlanguage.com");
                var request = new RestRequest("/0.2/detect", Method.POST);
                request.AddParameter("key", "b24a5753ff1d64c5b967952005028436");
                request.AddParameter("q", content);
                var response = client.Execute(request);
                var deserializer = new JsonDeserializer();
                var result = deserializer.Deserialize<Result>(response);
                var detection = result.Data.Detections[0];
                return detection.Language;
            }
            catch (Exception ex)
            {
                Logger.LogE("GetContentLanguage -> " + ex.Source + ex.Message);
                return "";
            }
        }

        public class Detection
        {
            public string Language { get; set; }
            public bool IsReliable { get; set; }
            public float Confidence { get; set; }
        }

        public class ResultData
        {
            public List<Detection> Detections { get; set; }
        }

        public class Result
        {
            public ResultData Data { get; set; }
        }
    }
}