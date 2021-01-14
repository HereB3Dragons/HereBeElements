namespace HereBeElements.Templates
{
    public interface IUpdatable<T>
    {
        void UpdateState(T data);
    }
}