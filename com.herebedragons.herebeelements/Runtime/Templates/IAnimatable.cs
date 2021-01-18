using HereBeElements.Templates;

namespace BetBluff.Table.interfaces
{
    public interface IAnimatable: IMoveable
    {
        bool IsAnimating();

        void SetAnimating(bool value);

    }
}