﻿using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Entity
{
    /// <summary>
    /// A sample entity type. Entity types are types of which the objects are persisted
    /// in the Mongo cache database. Entity types should be placed in the "Entity"-folder.
    /// </summary>
    public class Foo : ContentBase
    {
        public string DisplayName { get; set; }

        public bool IsBar { get; set; }

        public int Value { get; set; }

        public Foo() { }

        public Foo(FooArgs args)
        {
            DisplayName = args.DisplayName;
            IsBar = args.IsBar;
            Value = args.Value;
        }

        public FooArgs CreateFooArgs() => new FooArgs()
        {
            DisplayName = DisplayName,
            IsBar = IsBar,
            Value = Value
        };
    }
}
