using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace MongodbCs
{
    class Mongodb
    {
        MongoClient m_client;
        IMongoDatabase m_database;

        public Mongodb(string connectionString)
        {
            m_client = new MongoClient(connectionString);
        }

        public void Connect(string dbName)
        {
            m_database = m_client.GetDatabase(dbName);            
        }

        public void InsertDocument(string collectionName, BsonDocument document)
        {
            var collection = m_database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(document);
        }
       
    }
}
