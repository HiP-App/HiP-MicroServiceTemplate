using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model
{
    public static class ResourceTypes
    {
        public static ResourceType Foo { get; private set; }

        /// <summary>
        /// Initializes the fields
        /// </summary>
        public static void Initialize()
        {
            Foo = ResourceType.Register(nameof(Foo), typeof(FooArgs));
        }

    }
}
