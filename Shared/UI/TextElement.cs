using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceTrucker.Shared.UI;

namespace SpaceTrucker.Client
{
    public class TextElement : IElement
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsVisable { get; set; }
        public Rectangle Bounds { get; set; }
        public IElement Parent { get; set; }
        public Game Instance { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public TextElement(string name, string text, Rectangle bounds, IPanel parent, Game instance, SpriteFont font, Color color)
        {
            Name = name;
            Text = text;
            Bounds = bounds;
            Parent = parent;
            Instance = instance;
            Font = font;
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisable)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font, Text, new Vector2(Bounds.X, Bounds.Y), Color);
                spriteBatch.End();
            }
        }
        public void Move(Point point)
        {
            Bounds = new Rectangle(point, Bounds.Size);
        }
        public void SetTexture(string textureName)
        {
        }
        public void SetTexture(Texture2D texture)
        {
        }
        public void SetVisability(bool visable)
        {
            IsVisable = visable;
        }
        public void Update(GameTime gameTime)
        {
           
        }

        public Point GetRootNodePos()
        {
            throw new System.NotImplementedException();
        }
    }
}