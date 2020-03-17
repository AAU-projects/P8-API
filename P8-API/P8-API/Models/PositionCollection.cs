using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace P8_API.Models
{
    public class PositionCollection
    {
        [BsonId]
        public string UserId { get; set; }
        public List<Position> PositionList { get; set; }

        public PositionCollection(string userId, List<Position> positionList)
        {
            UserId = userId;
            PositionList = positionList;
        }
    }
}
