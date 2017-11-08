using PaderbornUniversity.SILab.Hip.EventSourcing;
using System;

namespace HiP_MicroServiceTemplate.Events
{
    /// <summary>
    /// An event for simple create, update and delete operations.
    /// </summary>
    public interface ICrudEvent : IEvent
    {
        /// <summary>
        /// Gets the type of the created, updated or deleted entity.
        /// </summary>
        ResourceType GetEntityType();

        /// <summary>
        /// The ID of the created, updated or deleted entity.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The date and time when the entity was created, updated or deleted.
        /// </summary>
        DateTimeOffset Timestamp { get; }
    }
    public interface ICreateEvent : ICrudEvent
    {
    }

    public interface IUpdateEvent : ICrudEvent
    {
    }

    public interface IDeleteEvent : ICrudEvent
    {
    }

    public interface IUserActivityEvent : ICrudEvent
    {
        /// <summary>
        /// The ID of the user that initiated the event.
        /// In creation-events, this is also known as the "entity owner".
        /// </summary>
        string UserId { get; }
    }
}
