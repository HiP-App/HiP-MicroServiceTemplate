namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Utility
{
    public class EndpointConfig
    {
        /// <summary>
        /// Connection string for the Mongo DB cache database.
        /// Default value: "mongodb://localhost:27017"
        /// </summary>
        public string MongoDbHost { get; set; } = "mongodb://localhost:27017";

        /// <summary>
        /// Name of the database to use.
        /// Default value: "main"
        /// </summary>
        public string MongoDbName { get; set; } = "main";
    }
}
