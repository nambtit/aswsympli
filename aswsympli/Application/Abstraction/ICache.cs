using Microsoft.Win32.SafeHandles;

namespace Application.Abstraction
{
    public interface ICache
    {
        void Get();
        void Set();
    }
}
