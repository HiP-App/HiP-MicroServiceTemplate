using System;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest
{
    /// <summary>
    /// The type of objects that are returned for Foo-queries.
    /// </summary>
    public class FooResult
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsBar { get; set; }

        public int Value { get; set; }

        public string UserId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

    }
}
