using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Infrastructure.StockPhotoAPI.Models;

namespace UserManagement.Infrastructure.StockPhotoAPI.Interfaces
{
    public interface IStockPhotoServices
    {
        Task<Photo> GetPhotoById(long id);

        Task<PhotoGallery> GetPhotos(int page = 1, int perPageCount = 10);
    }
}
