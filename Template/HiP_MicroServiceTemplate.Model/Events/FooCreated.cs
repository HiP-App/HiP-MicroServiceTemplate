using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest;
using System;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Events
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
