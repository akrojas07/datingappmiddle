using System;
using System.Collections.Generic;
using System.Text;

using MicroServicePhoto = MatchesManagement.Infrastructure.UserManagementAPI.Models.Photo;
using DomainPhoto = MatchesManagement.Domain.Models.Photo;

namespace MatchesManagement.Domain.AdoMapper
{
    public static class AdoPhotoMapper
    {
        public static MicroServicePhoto DomainToMSPhoto(DomainPhoto domainPhoto)
        {
            var msPhoto = new MicroServicePhoto()
            {
                URL = domainPhoto.URL,
                Id = domainPhoto.Id
            };

            return msPhoto;
        }

        public static DomainPhoto MSToDomainPhoto(MicroServicePhoto msPhoto)
        {
            var domainPhoto = new DomainPhoto()
            {
                Id = msPhoto.Id,
                URL = msPhoto.URL
            };

            return domainPhoto;
        }
    }
}
