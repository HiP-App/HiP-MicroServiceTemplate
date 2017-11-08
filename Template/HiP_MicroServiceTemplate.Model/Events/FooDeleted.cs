using System;

namespace HiP_MicroServiceTemplate.Events
{
    public class FooDeleted : IDeleteEvent, IUserActivityEvent
    {
        public int Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string UserId { get; set; }

        public ResourceType GetEntityType() => ResourceType.Foo;
    }
}
