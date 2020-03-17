using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P8_API.Models
{
    public class PositionDocument
    {
        [BsonId]
        public string DateId { get; set; }
        public List<Position> PositionList { get; set; }
        public PositionDocument(string dateId, List<Position> positionList)
        {
            DateId = dateId;
            PositionList = positionList;
        }

    }
}
