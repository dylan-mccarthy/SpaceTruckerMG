using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceTrucker.Shared.UI
{

    public class PanelHeader : IPanel
    {
        public string Name { get; set; }
        public IElement Parent { get; set; }
        private List<IElement> _elements;
        private Texture2D _texture;
        private bool _dragging;
        private Rectangle _bounds;
        public bool IsVisable { get; set; }
        public bool HasCloseButton { get; set; }

        public int HeaderSize { get; set; }

        private Game _game;

        public bool HasBorder { get; set;}
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public string HeaderText { get; set; }
        public Color HeaderTextColor { get; set; }

        private Rectangle _closeButtonArea;

        //TODO: Pull Event Handlers into the Shared Library
        public PanelHeader(string name,IElement Parent, Rectangle bounds, Game game, bool hasCloseButton, UIEventHandler eventHandler)
        {
            this.Name = name;
            this._elements = new List<IElement>();
            this._bounds = bounds;
            this._game = game;
            this.Parent = Parent as IPanel;
            this.HasCloseButton = hasCloseButton;

            if (hasCloseButton)
            {
                //Create new Button and add to Panel Header
                var closeButton = new Button("CloseButton", "", this);
                closeButton.SetTexture(this._game.Content.Load<Texture2D>("UI/icons8-close-48"));
                var buttonSize = closeButton.TextureSize;
                closeButton.Bounds = new Rectangle(_bounds.Width - (int)buttonSize.X/2, 0, (int)buttonSize.X/2, (int)buttonSize.Y/2);
                closeButton.HasBorder = false;
                closeButton.IsVisable = true;
                closeButton.OnButtonClicked += (sender, EventArgs) => eventHandler.Connect("Close Window", sender, EventArgs);
                _elements.Add(closeButton);
                _closeButtonArea = closeButton.Bounds;
            }
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

                Rectangle drawRect;

                drawRect = this.Bounds;
                
                if (HasBorder)
                {
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, HeaderSize), Color.White);
                    //Draw Border around all sides
                    //Top
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, this.BorderSize), this.BorderColor);
                    //Bottom
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y + drawRect.Height - this.BorderSize, drawRect.Width, this.BorderSize), this.BorderColor);
                    //Left
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
                    //Right
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X + drawRect.Width - this.BorderSize, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
                }
                else
                {
                    spriteBatch.Draw(_texture, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, HeaderSize), Color.White);
                }


                //Draw close button and scale to height of header including border center button vertically
                //var closeButtonTexture = this._game.Content.Load<Texture2D>("UI/icons8-close-48");
                //var closeButtonSize = new Vector2(HeaderSize - (this.BorderSize * 2), HeaderSize - (this.BorderSize * 2));
                //Calculate the position of the close button in relation to the panel header being drawn
                //var closeButtonPosition = new Vector2(drawRect.X + drawRect.Height - closeButtonSize.X,drawRect.Y + drawRect.Width);

                //spriteBatch.Draw(closeButtonTexture, closeButtonPosition, Color.White);
                //_closeButtonArea = new Rectangle((int)closeButtonPosition.X, (int)closeButtonPosition.Y, (int)closeButtonSize.X, (int)closeButtonSize.Y);

                //Draw Header Text align to left and keep within header bounds.
                //Scale text size if text is too long
                if (this.HeaderText != null)
                {
                    var font = this._game.Content.Load<SpriteFont>("UI/PanelHeaderFont");
                    var textSize = font.MeasureString(this.HeaderText);
                    var textPosition = new Vector2(this.Parent.Bounds.X + this.BorderSize, this.Parent.Bounds.Y + (HeaderSize / 2) - (textSize.Y / 2));
                    if (textSize.X > this.Parent.Bounds.Width - (HeaderSize * 2))
                    {
                        //Text is too long
                        //Scale text size
                        var scale = (this.Parent.Bounds.Width - (HeaderSize * 2)) / textSize.X;
                        spriteBatch.DrawString(font, this.HeaderText, textPosition, this.HeaderTextColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    }
                    else
                    {
                        //Text is not too long
                        //Draw text
                        spriteBatch.DrawString(font, this.HeaderText, textPosition, this.HeaderTextColor);
                    }
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

            Rectangle DragBounds;

            if(HasCloseButton)
            {
                DragBounds = new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width - this._closeButtonArea.Width, this.Bounds.Height);
            }
            else
            {
                DragBounds = this.Bounds;
            }
     
            if (DragBounds.Contains(mouseState.Position))
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
            //Move Panel
            if (point.X < 0)
            {
                point.X = 0;
            }
            if (point.Y < 0)
            {
                point.Y = 0;
            }
            if (point.X + this.Bounds.Width > this.Instance.GraphicsDevice.Viewport.Width)
            {
                point.X = this.Instance.GraphicsDevice.Viewport.Width - this.Bounds.Width;
            }
            if (point.Y + this.Bounds.Height > this.Instance.GraphicsDevice.Viewport.Height)
            {
                point.Y = this.Instance.GraphicsDevice.Viewport.Height - this.Bounds.Height;
            }

            //Calculate new positions for all children
            var oldPosition = this.Bounds.Location;
            var newPosition = point;
            var delta = new Point(newPosition.X - oldPosition.X, newPosition.Y - oldPosition.Y);
            foreach (var element in this._elements)
            {
                element.Move(new Point(element.Bounds.X + delta.X, element.Bounds.Y + delta.Y));
            }

            this.Bounds = new Rectangle(point.X, point.Y, this.Bounds.Width, this.Bounds.Height);
        }

        public Rectangle Bounds
        {
            get => this._bounds;
            set => this._bounds = value;
        }
        public Game Instance
        {
            get => this._game;
            set => this._game = value;
        }
    }
}