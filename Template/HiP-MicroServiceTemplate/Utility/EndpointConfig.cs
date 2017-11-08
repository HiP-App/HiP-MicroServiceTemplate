namespace HiP_MicroServiceTemplate.Utility
{
    public class EndpointConfig
    {
        /// <summary>
        /// Connection string for the Mongo DB cache database.
        /// Example: "mongodb://localhost:27017"
        /// </summary>
        public string MongoDbHost { get; set; }

        /// <summary>
        /// Name of the database to use.
        /// Example: "main"
        /// </summary>
        public string MongoDbName { get; set; }
    }
}
