using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace P8_API.Models
{
    [ExcludeFromCodeCoverage]
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
