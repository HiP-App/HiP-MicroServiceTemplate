﻿namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Entity
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
