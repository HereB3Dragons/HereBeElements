namespace com.herebedragons.herebeelements.Runtime.Templates
{
    public interface IHighlight
    {
        bool IsHighlight();
        
        void Highlight(bool onOff = true);

        void DeHighlight();
    }
}