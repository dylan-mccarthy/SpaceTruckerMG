using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceTrucker.Shared.UI
{
    public class Panel : IPanel
    {
        public Panel()
        {
            this._elements = new List<IElement>();
        }

        public void AddElement(IElement element)
        {
            this._elements.Add(element);
        }

        public void RemoveElement(IElement element)
        {
            this._elements.Remove(element);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Panel then Draw Children
            if (this._texture != null)
            {
                spriteBatch.Begin();
                //Draw Panel with or without border
                if (this.HasBorder)
                {
                    //Draw main panel area without border around all sides
                    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X + this.BorderSize, this.Bounds.Y + this.BorderSize, this.Bounds.Width - (this.BorderSize * 2), this.Bounds.Height - (this.BorderSize * 2)), Color.White);
                    
                    //Draw Border around all sides
                    //Top
                    //If panel has PanelHeader draw border below header
                    var panelHeader = this._elements.Find(x => x.GetType() == typeof(PanelHeader));
                    if (panelHeader != null)
                    {
                        spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y + panelHeader.Bounds.Height, this.Bounds.Width, this.BorderSize), this.BorderColor);
                    }
                    else
                    {
                        spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.BorderSize), this.BorderColor);
                    }
                    //Bottom
                    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y + this.Bounds.Height - this.BorderSize, this.Bounds.Width, this.BorderSize), this.BorderColor);
                    //Left
                    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y, this.BorderSize, this.Bounds.Height), this.BorderColor);
                    //Right
                    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X + this.Bounds.Width - this.BorderSize, this.Bounds.Y, this.BorderSize, this.Bounds.Height), this.BorderColor);
                }
                else
                {
                    spriteBatch.Draw(this._texture, this.Bounds, Color.White);
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
            foreach (var element in this._elements)
            {
                element.Update(gameTime);
            }
        }

        public void SetTexture(string textureName)
        {
            this._texture = Instance.Content.Load<Texture2D>(textureName);
        }

        public void SetTexture(Texture2D texture)
        {
            this._texture = texture;
        }

        public void Move(Point point){
            //Move Panel and all Children to point and keep inside game window
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
            this.Bounds = new Rectangle(point.X, point.Y, this.Bounds.Width, this.Bounds.Height);

            foreach (var element in this._elements)
            {
                element.Move(point);
            }
        }

        public void SetVisability(bool visable)
        {
            //Set Visability of Panel and all Children except PanelHeader
            this.IsVisable = visable;
            foreach (var element in this._elements)
            {
                if (element.GetType() != typeof(PanelHeader))
                {
                    element.SetVisability(visable);
                }
            }
        }

        private List<IElement> _elements;
        private Texture2D _texture;
        private Game Instance { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsVisable { get; set; }
        public bool HasBorder { get; set;}
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public IPanel Parent { get; set; }

        public Panel(Rectangle bounds, Game instance)
        {
            this.Bounds = bounds;
            this._elements = new List<IElement>();
            this._texture = null;
            this.Instance = instance;
        }
    }
}