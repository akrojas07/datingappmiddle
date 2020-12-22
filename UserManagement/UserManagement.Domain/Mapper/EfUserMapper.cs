using System;
using System.Collections.Generic;
using System.Text;
using CoreModel = UserManagement.Domain.Models.UserModel;
using DbEntity = UserManagement.Infrastructure.Persistence.Entities.User;

namespace UserManagement.Domain.Mapper
{
    public static class EfUserMapper
    {
        public static CoreModel DbEntityToCoreModel(this DbEntity dbEntity)
        {
            CoreModel coreModel = new CoreModel()
            {
                Id = dbEntity.Id,
                FirstName = dbEntity.FirstName,
                LastName = dbEntity.LastName,
                Username = dbEntity.Username,
                Password = dbEntity.Password,
                Location = dbEntity.Location,
                Interests = dbEntity.Interests,
                Gender = (bool)dbEntity.Gender,
                About = dbEntity.About
    };

            return coreModel;
        }

        public static DbEntity CoreModelToDbEntity(this CoreModel coreModel)
        {
            DbEntity dbEntity = new DbEntity()
            {
                FirstName = coreModel.FirstName,
                LastName = coreModel.LastName,
                Username = coreModel.Username,
                Password = coreModel.Password,
                Id = coreModel.Id,
                PhotoId = coreModel.Photo.Id,
                Location = coreModel.Location,
                About = coreModel.About,
                Interests = coreModel.Interests,
                Gender = coreModel.Gender
            };

            return dbEntity;
        }
    }
}
