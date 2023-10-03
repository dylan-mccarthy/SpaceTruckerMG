using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker.Shared.UI
{
    public class Button : IButton
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsPressed { get; set; }
        public bool IsHovered { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisable { get; set; }
        public Rectangle Bounds { get; set; }

        public IElement Parent { get; set; }
        public bool HasBorder { get; set; }
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public event EventHandler<ButtonClickedArgs> ButtonClicked;

        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);

        private Texture2D _texture;

        public Vector2 TextureSize;
        public Game Instance { get; set; }

        public Button(string name, string text, IPanel parent)
        {
            this.Name = name;
            this.Text = text;
            this.Parent = parent;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Button with Text
            if (this.IsVisable)
            {
                spriteBatch.Begin();
                Rectangle drawRect;

                drawRect = this.Bounds;
                
                //Draw Button in relation to parent
                spriteBatch.Draw(_texture, drawRect, Color.White);
                if (Text != "")
                {
                    //Draw Text in Center of button both vertically and horizontally
                    var font = Parent.Instance.Content.Load<SpriteFont>("Fonts/Default");
                    var textSize = font.MeasureString(this.Text);
                    var textPosition = new Vector2(drawRect.X + (drawRect.Width / 2) - (textSize.X / 2), drawRect.Y + (drawRect.Height / 2) - (textSize.Y / 2));
                    spriteBatch.DrawString(font, this.Text, textPosition, Color.Black);
                }
                //Draw Border around all sides
                if (this.HasBorder)
                {
                    //Top
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, this.BorderSize), this.BorderColor);
                    //Bottom
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y + drawRect.Height - this.BorderSize, drawRect.Width, this.BorderSize), this.BorderColor);
                    //Left
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
                    //Right
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X + drawRect.Width - this.BorderSize, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
                }
                spriteBatch.End();
            }
        }

        public void Update(GameTime gameTime)
        {
            //Update Button State
            if (this.IsVisable)
            {
                var mouseState = Mouse.GetState();
                var mousePoint = new Point(mouseState.X, mouseState.Y);
                if (this.Bounds.Contains(mousePoint))
                {
                    this.IsHovered = true;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        this.IsPressed = true;
                    }
                    else
                    {
                        if (this.IsPressed)
                        {
                            this.IsPressed = false;
                            OnButtonClicked(this, new ButtonClickedArgs(this, this.Text));
                        }
                    }
                }
                else
                {
                    this.IsHovered = false;
                    this.IsPressed = false;
                }
            }
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {
            ButtonClicked?.Invoke(sender, (ButtonClickedArgs)e);
        }

        public void Move(Point point)
        {
            this.Bounds = new Rectangle(point.X, point.Y, this.Bounds.Width, this.Bounds.Height);
        }

        public void SetTexture(string textureName)
        {
            _texture = Parent.Instance.Content.Load<Texture2D>(textureName);
            //Set Texture Size
            this.TextureSize = new Vector2(_texture.Width, _texture.Height);
        }

        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
            //Set Texture Size
            this.TextureSize = new Vector2(_texture.Width, _texture.Height);
        }

        public void SetVisability(bool visable)
        {
            this.IsVisable = visable;
        }
    }

    public class ButtonClickedArgs : EventArgs
    {
        public Button Button { get; set; }
        public string Text { get; set; }
        public ButtonClickedArgs(Button button, string text)
        {
            Button = button;
            Text = text;
        }
    }
}
