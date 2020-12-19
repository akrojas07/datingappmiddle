using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.StockPhotoAPI.Interfaces;
using UserManagement.Infrastructure.StockPhotoAPI.Models;
using System.Net;
using Newtonsoft.Json;
using UserManagement.Infrastructure.Http;

namespace UserManagement.Infrastructure.StockPhotoAPI
{
    public class StockPhotoServices: IStockPhotoServices
    { 
        private readonly string _baseUrl;
        private readonly string _key;

        private readonly IHttpClientService _httpClientService;
        public StockPhotoServices(IConfiguration configuration, IHttpClientService httpClientService)
        {
            _baseUrl = configuration.GetSection("Pexel:URL").Value;
            _key = configuration.GetSection("Pexel:Key").Value;

            _httpClientService = httpClientService;
        }

        public async Task<Photo> GetPhotoById(long id)
        {
            return await _httpClientService.GetAsync<Photo>($"{_baseUrl}/photo/{id}", _key, "Unable to find photo object");     
        }

        public async Task<PhotoGallery> GetPhotos(int page = 1, int perPageCount = 10)
        {
            return await _httpClientService.GetAsync<PhotoGallery>($"{_baseUrl}/search?query=people&per_page={perPageCount}&page={page}", _key, "Unable to photo gallery");
        }
    }
}
