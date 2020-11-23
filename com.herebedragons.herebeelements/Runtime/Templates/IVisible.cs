namespace HereBeElements.Templates
{
    public interface IVisible
    {
        bool IsVisible();
        
        void Show(bool onOff = true);

        void Hide();
    }
}