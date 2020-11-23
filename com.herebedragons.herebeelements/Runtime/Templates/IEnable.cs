namespace HereBeElements.Templates
{
    public interface IEnable
    {
        bool IsEnabled();
        
        void Enable(bool onOff = true);

        void Disable();
    }
}