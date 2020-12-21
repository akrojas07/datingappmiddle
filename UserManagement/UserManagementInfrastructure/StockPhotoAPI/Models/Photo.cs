using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Infrastructure.StockPhotoAPI.Models
{
    public class Photo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("width")]
        public int Width  { get; set; }

        [JsonProperty("height")]
        public int Height  { get; set; }

        [JsonProperty("url")]
        public string URL  { get; set; }
        
        [JsonProperty("photographer")]
        public string Photographer { get; set; }
        
        [JsonProperty("photographer_url")]
        public string PhotographerUrl  { get; set; }
        
        [JsonProperty("photographer_id")]
        public  int PhotographerId  { get; set; }
        
        [JsonProperty("avg_color")]
        public string AvgColor  { get; set; }

        [JsonProperty("src")]
        public Src Source { get; set; }

    }
}
