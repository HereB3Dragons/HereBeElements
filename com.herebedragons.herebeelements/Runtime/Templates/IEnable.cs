namespace HereBeElements.Templates
{
    public interface IEnable
    {
        bool IsEnabled();
        
        void Enable(bool onOff = true, bool force = false);

        void Disable();
    }
}