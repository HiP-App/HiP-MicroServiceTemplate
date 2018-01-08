using System.ComponentModel.DataAnnotations;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest
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
