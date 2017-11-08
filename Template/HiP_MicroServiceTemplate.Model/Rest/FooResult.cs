namespace HiP_MicroServiceTemplate.Rest
{
    /// <summary>
    /// The type of objects that are returned for Foo-queries.
    /// </summary>
    public class FooResult
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsBar { get; set; }
    }
}
