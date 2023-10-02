using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceTrucker.Shared.UI
{

    public class PanelHeader : IPanel
    {
        public IPanel Parent { get; set; }
        private List<IElement> _elements;
        private Texture2D _texture;
        private bool _dragging;
        private Rectangle _bounds;
        public bool IsVisable { get; set; }

        public int HeaderSize { get; set; }

        private readonly Game _game;

        public bool HasBorder { get; set;}
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public string HeaderText { get; set; }
        public Color HeaderTextColor { get; set; }


        public PanelHeader(Rectangle bounds, Game game)
        {
            this._elements = new List<IElement>();
            this._bounds = bounds;
            this._game = game;
        }

        public void AddElement(IElement element)
        {
            this._elements.Add(element);
        }

        public void RemoveElement(IElement element)
        {
            this._elements.Remove(element);
        }

        public void SetVisability(bool visable)
        {
            //Set Panel Visability and all Children to visable
            foreach (var element in this._elements)
            {
                element.SetVisability(visable);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Panel header at top of Parent Panel with the same width and 20px height
            if (this._texture != null)
            {
                spriteBatch.Begin();
                if(HasBorder)
                {
                    //Draw main panel area without border around all sides
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X + this.BorderSize, this.Parent.Bounds.Y + this.BorderSize, this.Parent.Bounds.Width - (this.BorderSize * 2), HeaderSize - (this.BorderSize * 2)), Color.White);

                    //Draw Border
                    //Top
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X, this.Parent.Bounds.Y, this.Parent.Bounds.Width, this.BorderSize), this.BorderColor);
                    //Bottom
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X, this.Parent.Bounds.Y + HeaderSize - this.BorderSize, this.Parent.Bounds.Width, this.BorderSize), this.BorderColor);
                    //Left
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X, this.Parent.Bounds.Y, this.BorderSize, HeaderSize), this.BorderColor);
                    //Right
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X + this.Parent.Bounds.Width - this.BorderSize, this.Parent.Bounds.Y, this.BorderSize, HeaderSize), this.BorderColor);

                }
                else
                {
                    spriteBatch.Draw(this._texture, new Rectangle(this.Parent.Bounds.X, this.Parent.Bounds.Y, this.Parent.Bounds.Width, HeaderSize), Color.White);
                }

                //Draw Header Text
                if (this.HeaderText != null)
                {
                    // var font = this._game.Content.Load<SpriteFont>("DefaultFont");
                    // var textSize = font.MeasureString(this.HeaderText);
                    // var textPosition = new Vector2(this.Parent.Bounds.X + (this.Parent.Bounds.Width / 2) - (textSize.X / 2), this.Parent.Bounds.Y + (HeaderSize / 2) - (textSize.Y / 2));
                    // spriteBatch.DrawString(font, this.HeaderText, textPosition, this.HeaderTextColor);
                }
                spriteBatch.End();
            }

            foreach (var element in this._elements)
            {
                element.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            //Check if mouse is over panel
            var mouseState = Mouse.GetState();
            if (this.Bounds.Contains(mouseState.Position))
            {
                //Mouse is over panel
                //Check if mouse button is down
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //Mouse is down
                    //Check if dragging
                    if (this._dragging)
                    {
                        //Dragging
                        //Move Panel to mouse position
                        var mousePosition = mouseState.Position;
                        var x = mousePosition.X - this._bounds.Width / 2;
                        var y = mousePosition.Y - this._bounds.Height / 2;
                        this.Parent.Move(new Point(x, y));

                    }
                    else
                    {
                        //Not dragging
                        //Set dragging to true
                        this._dragging = true;
                    }
                }
                else
                {
                    //Mouse is up
                    //Set dragging to false
                    this._dragging = false;
                }

            }
            else
            {
                //Mouse is not over panel
                //Set dragging to false
                this._dragging = false;
            }

            foreach (var element in this._elements)
            {
                element.Update(gameTime);
            }
        }
        public void SetTexture(string textureName)
        {
            this._texture = _game.Content.Load<Texture2D>(textureName);
        }

        public void SetTexture(Texture2D texture)
        {
            this._texture = texture;
        }

        public void Move(Point point)
        {
            //Move Panel and all Children to point
            this.Bounds = new Rectangle(point.X, point.Y, this.Bounds.Width, this.Bounds.Height);
            foreach (var element in this._elements)
            {
                element.Move(point);
            }
        }

        public Rectangle Bounds
        {
            get => this._bounds;
            set => this._bounds = value;
        }
    }
}