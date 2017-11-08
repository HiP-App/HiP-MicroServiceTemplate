using System.ComponentModel.DataAnnotations;

namespace HiP_MicroServiceTemplate.Rest
{
    /// <summary>
    /// Specifies the parameters for creating new Foo-resources.
    /// </summary>
    public class FooArgs
    {
        [Required]
        public string DisplayName { get; set; }

        public bool IsBar { get; set; }
    }
}
