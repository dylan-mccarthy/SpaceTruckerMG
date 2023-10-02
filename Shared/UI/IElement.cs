using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceTrucker.Shared.UI
{
    public interface IElement
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void Move(Point point);
        void SetTexture(string textureName);
        void SetTexture(Texture2D texture);
        void SetVisability(bool visable);

        public bool IsVisable { get; set; }
        public Rectangle Bounds { get; set; }
    }
}