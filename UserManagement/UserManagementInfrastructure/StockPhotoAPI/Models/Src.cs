using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Infrastructure.StockPhotoAPI.Models
{
    public class Src
    {
        [JsonProperty("original")]
        public string Original { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }

        [JsonProperty("large2x")]
        public string Large2x { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("small")]
        public string Small { get; set; }

        [JsonProperty("portrait")]
        public string Portrait { get; set; }

        [JsonProperty("landscape")]
        public string Landscape { get; set; }

        [JsonProperty("tiny")]
        public string Tiny { get; set; }
    }
}
