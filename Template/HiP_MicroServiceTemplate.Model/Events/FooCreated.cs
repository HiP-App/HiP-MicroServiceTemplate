using HiP_MicroServiceTemplate.Rest;
using System;

namespace HiP_MicroServiceTemplate.Events
{
    public class FooCreated : ICreateEvent, IUserActivityEvent
    {
        public int Id { get; set; }

        public FooArgs Properties { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string UserId { get; set; }

        public ResourceType GetEntityType() => ResourceType.Foo;
    }
}
