using MongoDB.Bson;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repository;

public interface INoteMongoDbRepository : IRepository<Note, ObjectId>
{
    
}