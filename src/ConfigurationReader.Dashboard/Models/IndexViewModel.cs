using System.Collections.Generic;
using ConfigurationReader.Abstraction;
using MongoDB.Bson;

namespace ConfigurationReader.Dashboard.Models {
    public class IndexViewModel {
        public IEnumerable<IConfiguration<ObjectId>> Configurations { get; set; }
    }
}