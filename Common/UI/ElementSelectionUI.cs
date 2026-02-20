using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spellbound.Data;
using Spellbound.Data.Magic;
using Spellbound.Loaders;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellbound.Common.UI
{
    public class ElementSelectionUi : UIState
    {
        // Vertical scroll offset
        private float scrollOffset = 0f;

        // Selected element
        private Element selectedElement = null;

        // UI panel dimensions
        private const int frameWidth = 160;
        private const int frameHeight = 105;
        private const int scrollbarWidth = 12;

        // Grid layout
        private const int columns = 2;
        private const int iconWidth = 38;
        private const int iconHeight = 68;
        private const int hSpacing = 24; // horizontal spacing between icons and edges
        private const int vSpacing = 12; // vertical spacing between icons and edges

        private List<Element> Elements => ElementLoader.LoadedContent;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            string path = GetType().Namespace;
            if (path != null)
            {
                path = path.Replace('.', '/');
                Texture2D panelTexture = ModContent.Request<Texture2D>($"{path}/ElementFrame").Value;
                spriteBatch.Draw(panelTexture, new Rectangle(0, 0, frameWidth, frameHeight), Color.White);
            }
        }
    }
}
