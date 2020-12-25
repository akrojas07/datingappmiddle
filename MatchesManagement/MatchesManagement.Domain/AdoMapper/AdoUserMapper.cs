using System;
using System.Collections.Generic;
using System.Text;

using MicroServiceUser = MatchesManagement.Infrastructure.UserManagementAPI.Models.User;
using DomainUser = MatchesManagement.Domain.Models.User;
using MatchesManagement.Domain.Models;

namespace MatchesManagement.Domain.AdoMapper
{
    public static class AdoUserMapper
    {
        public static MicroServiceUser DomainToMsUser(DomainUser domainUser)
        {
            var msUser = new MicroServiceUser()
            {
                Id = domainUser.Id,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                About = domainUser.About,
                Interests = domainUser.Interests,
                Gender = domainUser.Gender,
                Location = domainUser.Location,
                Username = domainUser.Username,
                Photo = AdoMapper.AdoPhotoMapper.DomainToMSPhoto(domainUser.Photo)
            };

            return msUser;

        }

        public static DomainUser MsUserToDomainUser(MicroServiceUser msUser)
        {
            var domainUser = new DomainUser()
            {
                Id = msUser.Id,
                FirstName = msUser.FirstName,
                LastName = msUser.LastName,
                About = msUser.About,
                Interests = msUser.Interests,
                Gender = msUser.Gender,
                Location = msUser.Location,
                Username = msUser.Username,
                Photo = AdoPhotoMapper.MSToDomainPhoto(msUser.Photo)

            };

            return domainUser;
        }
    }
}
