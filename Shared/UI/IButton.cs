using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrucker.Shared.UI
{
    public interface IButton : IElement
    {
        public string Text { get; set; }
        public bool IsPressed { get; set; }
        public bool IsHovered { get; set; }
        public bool IsDisabled { get; set; }
        public bool HasBorder { get; set; }
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);

        public void OnButtonClicked(object sender, EventArgs e);

    }
}
