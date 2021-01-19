namespace HereBeElements.UI
{
    public class SliderButton: UIElementGroup
    {
        public Operation operation;
        public float delta = 1;

        public float GetDelta()
        {
            return Operation.PLUS == operation ? delta : -delta;
        }
    }

    public enum Operation
    {
        MINUS,
        PLUS
    }
    
}