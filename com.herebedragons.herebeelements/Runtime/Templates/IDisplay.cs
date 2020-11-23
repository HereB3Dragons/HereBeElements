using Internal;

namespace com.herebedragons.herebeelements.Runtime.Templates
{
    public interface IDisplay
    {
        void Show(string text = null);

        void Show(long value);

        void Clear(); 
    }
}