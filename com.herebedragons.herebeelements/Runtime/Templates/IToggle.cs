using System;

namespace BetBluff.Table.interfaces
{
    public interface IToggle: IAnimatable
    {
        void Toggle(Action callback = null);
    }
}