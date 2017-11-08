using HiP_MicroServiceTemplate.Model.Entity;
using System;

namespace HiP_MicroServiceTemplate.Entity
{
    public abstract class ContentBase : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Owner of the content.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The date and time of the last modification.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
