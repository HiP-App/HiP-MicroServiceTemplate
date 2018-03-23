using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.Events;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using PaderbornUniversity.SILab.Hip.EventSourcing.Mongo;
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
        private readonly IMongoDbContext _db;

        public IMongoDbContext Database => _db;

        public CacheDatabaseManager(
            EventStoreService eventStore,
            IMongoDbContext db)
        {
            // For now, the cache database is always created from scratch by replaying all events.
            // This also implies that, for now, the cache database always contains the entire data (not a subset).
            // In order to receive all the events, a Catch-Up Subscription is created.
            _db = db;

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
                            _db.Add(ResourceTypes.Foo, foo);
                            break;
                    }

                    break;

                case PropertyChangedEvent e:
                    resourceType = e.GetEntityType();
                    switch (resourceType)
                    {
                        case ResourceType _ when resourceType == ResourceTypes.Foo:
                            var originalFoo = _db.GetCollection<Foo>(resourceType).AsQueryable().First(x => x.Id == e.Id);
                            var args = originalFoo.CreateFooArgs();
                            e.ApplyTo(args);
                            var updatedFoo = new Foo(args)
                            {
                                Id = e.Id,
                                Timestamp = e.Timestamp,
                                UserId = originalFoo.UserId
                            };
                            _db.Replace((ResourceTypes.Foo, e.Id), updatedFoo);
                            break;
                    }

                    break;


                case DeletedEvent e:
                    resourceType = e.GetEntityType();
                    switch (resourceType)
                    {
                        case ResourceType _ when resourceType == ResourceTypes.Foo:
                            _db.Delete((ResourceTypes.Foo, e.Id));
                            break;
                    }

                    break;
            }
        }
    }
}
