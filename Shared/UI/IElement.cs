using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceTrucker.Shared.UI
{
    public interface IElement
    {
        public string Name { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void Move(Point point);
        void SetTexture(string textureName);
        void SetTexture(Texture2D texture);
        void SetVisability(bool visable);
        public IElement Parent { get; set; }
        public bool IsVisable { get; set; }
        public Rectangle Bounds { get; set; }
        Game Instance { get; set; }
    }
}