
namespace HereBeElements.Templates
{
    public interface IDisplay
    {
        void Show(string text = null);

        void Show(long value);

        void Clear(); 
    }
}