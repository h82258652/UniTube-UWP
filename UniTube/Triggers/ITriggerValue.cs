using System;

namespace UniTube.Triggers
{
    public interface ITriggerValue
    {
        bool IsActive { get; }
        event EventHandler IsActiveChanged;
    }
}
