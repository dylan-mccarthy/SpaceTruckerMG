
using Microsoft.Xna.Framework;

namespace SpaceTrucker.Shared.UI
{
    public interface IPanel : IElement
    {
        void AddElement(IElement element);
        void RemoveElement(IElement element);
    }
}