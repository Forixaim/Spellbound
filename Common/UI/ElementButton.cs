using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spellbound.Data.Magic;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellbound.Common.UI
{
    internal class ElementButton : UIElement
    {
        public Element Element { get; private set; }
        public Texture2D Texture => Element.Icon;

        private bool _hovered = false;
        private readonly int _width = 38;
        private readonly int _height = 68;

        public delegate void OnClick(Element element);
        private readonly OnClick clickAction;

        public ElementButton(Element element, int width, int height, OnClick onClick)
        {
            this.Element = element;
            this._width = width;
            this._height = height;
            this.clickAction = onClick;

            Width.Set(width, 0f);
            Height.Set(height, 0f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 pos = GetDimensions().Position();
            spriteBatch.Draw(Texture, pos, Color.White);

            if (_hovered)
            {
                // Draw a simple highlight overlay
                Texture2D highlight = ModContent.Request<Texture2D>("Terraria/Images/UI/MouseOver").Value;
                spriteBatch.Draw(highlight, new Rectangle((int)pos.X, (int)pos.Y, _width, _height), Color.Yellow * 0.5f);
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            _hovered = true;
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            _hovered = false;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            clickAction?.Invoke(Element);
        }
    }
}
