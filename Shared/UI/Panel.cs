using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceTrucker.Shared.UI
{
    public class Panel : IPanel
    {
        private List<IElement> _elements;
        private Texture2D _texture;
        public string Name { get; set; }
        public Game Instance { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsVisable { get; set; }
        public bool HasBorder { get; set; }
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public IElement Parent { get; set; }

        public Panel(string name, Rectangle bounds, Game instance, IElement Parent)
        {
            this.Name = name;
            this.Bounds = bounds;
            this._elements = new List<IElement>();
            this._texture = null;
            this.Instance = instance;
            this.IsVisable = true;
            this.Parent = Parent;
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
            if (IsVisable)
            {
                //Draw Panel then Draw Children
                if (this._texture != null)
                {
                    var drawRect = this.Bounds;

                    spriteBatch.Begin();
                    //Draw Panel with or without border
                    if (this.HasBorder)
                    {
                        //Draw main panel area without border around all sides
                        spriteBatch.Draw(this._texture, drawRect, Color.White);

                        //Draw Border around all sides
                        //Top
                        //If panel has PanelHeader draw border below header
                        //var panelHeader = this._elements.Find(x => x.GetType() == typeof(PanelHeader));
                        //if (panelHeader != null)
                        //{
                        //    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y + panelHeader.Bounds.Height, this.Bounds.Width, this.BorderSize), this.BorderColor);
                        //}
                        //else
                        //{
                        //    spriteBatch.Draw(this._texture, new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.BorderSize), this.BorderColor);
                        //}

                        //Draw Borders
                        //Top
                        spriteBatch.Draw(this._texture, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, this.BorderSize), this.BorderColor);
                        //Bottom
                        spriteBatch.Draw(this._texture, new Rectangle(drawRect.X, drawRect.Y + drawRect.Height - this.BorderSize, drawRect.Width, this.BorderSize), this.BorderColor);
                        //Left
                        spriteBatch.Draw(this._texture, new Rectangle(drawRect.X, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
                        //Right
                        spriteBatch.Draw(this._texture, new Rectangle(drawRect.X + drawRect.Width - this.BorderSize, drawRect.Y, this.BorderSize, drawRect.Height), this.BorderColor);
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
        }

        public Point GetRootNodePos()
        {
            Point position = new Point();
            IElement current = this;
            while (current != null && current.Parent != null)
            {
                position.X += current.Bounds.X;
                position.Y += current.Bounds.Y;
                current = current.Parent;
            }
            return position;
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

    }
}