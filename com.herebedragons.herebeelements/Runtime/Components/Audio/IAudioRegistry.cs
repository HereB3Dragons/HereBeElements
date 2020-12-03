
namespace HereBeElements.Audio
{
    public interface IAudioRegistry<T> 
    {
        void Register(T element);

        void Unregister(T element);
    }
}