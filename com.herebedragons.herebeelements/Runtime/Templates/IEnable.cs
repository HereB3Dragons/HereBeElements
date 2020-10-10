namespace com.herebedragons.herebeelements.Runtime.Templates
{
    public interface IEnable
    {
        bool IsEnabled();
        
        void Enable(bool onOff = true);

        void Disable();
    }
}