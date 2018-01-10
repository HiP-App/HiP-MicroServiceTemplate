using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.Events;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Entity;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Utility;
using System;
using System.Linq;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Core
{
    /// <summary>
    /// Subscribes to EventStore events to keep the cache database up to date.
    /// </summary>
    public class CacheDatabaseManager
    {
        private readonly EventStoreService _eventStore;
        private readonly IMongoDatabase _db;

        public IMongoDatabase Database => _db;

        public CacheDatabaseManager(
            EventStoreService eventStore,
            IOptions<EndpointConfig> config,
            ILogger<CacheDatabaseManager> logger)
        {
            // For now, the cache database is always created from scratch by replaying all events.
            // This also implies that, for now, the cache database always contains the entire data (not a subset).
            // In order to receive all the events, a Catch-Up Subscription is created.

            // 1) Open MongoDB connection and clear existing database
            var mongo = new MongoClient(config.Value.MongoDbHost);
            mongo.DropDatabase(config.Value.MongoDbName);
            _db = mongo.GetDatabase(config.Value.MongoDbName);
            var uri = new Uri(config.Value.MongoDbHost);
            logger.LogInformation($"Connected to MongoDB cache database on '{uri.Host}', using database '{config.Value.MongoDbName}'");

            // 2) Subscribe to EventStore to receive all past and future events
            _eventStore = eventStore;
            _eventStore.EventStream.SubscribeCatchUp(ApplyEvent);
        }

        private void ApplyEvent(IEvent ev)
        {
            // TODO: Handle incoming events by updating the Mongo database
            switch (ev)
            {
                case CreatedEvent e:
                    var resourceType = e.GetEntityType();
                    switch (resourceType)
                    {
                        case ResourceType _ when resourceType == ResourceTypes.Foo:
                            var foo = new Foo
                            {
                                Id = e.Id,
                                Timestamp = e.Timestamp,
                                UserId = e.UserId
                            };
                            _db.GetCollection<Foo>(ResourceTypes.Foo.Name).InsertOne(foo);
                            break;
                    }

                    break;

                case PropertyChangedEvent e:
                    resourceType = e.GetEntityType();
                    switch (resourceType)
                    {
                        case ResourceType _ when resourceType == ResourceTypes.Foo:
                            var originalFoo = _db.GetCollection<Foo>(resourceType.Name).AsQueryable().First(x => x.Id == e.Id);
                            var args = originalFoo.CreateFooArgs();
                            e.ApplyTo(args);
                            var updatedFoo = new Foo(args)
                            {
                                Id = e.Id,
                                Timestamp = e.Timestamp,
                                UserId = e.UserId
                            };
                            _db.GetCollection<Foo>(resourceType.Name).ReplaceOne(x => x.Id == e.Id, updatedFoo);
                            break;
                    }

                    break;


                case DeletedEvent e:
                    resourceType = e.GetEntityType();
                    switch (resourceType)
                    {
                        case ResourceType _ when resourceType == ResourceTypes.Foo:
                            _db.GetCollection<Foo>(ResourceTypes.Foo.Name).DeleteOne(x => x.Id == e.Id);
                            break;
                    }

                    break;
            }
        }
    }
}
