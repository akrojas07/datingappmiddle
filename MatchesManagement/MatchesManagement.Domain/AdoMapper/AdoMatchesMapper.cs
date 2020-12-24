using System;
using System.Collections.Generic;
using System.Text;

using DbEntity = MatchesManagement.Infrastructure.Persistence.Entities.Matches;
using CoreModel = MatchesManagement.Domain.Models.Match;

namespace MatchesManagement.Domain.AdoMapper
{
    public static class AdoMatchesMapper
    {
        public static CoreModel DbEntityToCoreModel(this DbEntity dbEntity)
        {
            var coreModel = new CoreModel() 
            { 
                Id = dbEntity.MatchId,
                FirstUserId = dbEntity.FirstUserId,
                SecondUserId = dbEntity.SecondUserId,
                Liked = dbEntity.Liked,
                Matched = dbEntity.Matched
            };

            return coreModel;
        }

        public static DbEntity CoreModelToDbEntity(this CoreModel coreModel)
        {
            var dbEntity = new DbEntity()
            {
                MatchId = coreModel.Id,
                FirstUserId = coreModel.FirstUserId,
                SecondUserId = coreModel.SecondUserId,
                Liked = coreModel.Liked,
                Matched = coreModel.Matched
            };

            return dbEntity;
        }
    }
}
