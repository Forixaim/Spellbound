using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellbound.Data;
using Spellbound.Data.Magic;
using Spellbound.Loaders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellbound.Common.UI
{
    public class ElementSelectionUi : UIState
    {
        public static readonly Color DesaturatedBlue = new Color(63, 82, 151, 178);
        private List<Element> _selectedElements;
        private bool _selectedPath;
        private UIList _elements = new UIList();
        private UIPanel _panel = new UIPanel();

        public void Unload()
        {
            //TODO: Handle future Unload tasks.
        }

        private UIList AddElements()
        {
            UIList list = new UIList();
            list.Width.Set(50, 0.2f);
            list.Height.Set(50, 0.3f);
            

            return list;
        }

        public override void OnInitialize()
        {
            _panel.Width.Set(300, 0.7f);
            _panel.Height.Set(300, 0.4f);
            _panel.HAlign = 0.5f;
            _panel.VAlign = 0.5f;
            Append(_panel);
        }

        public override void OnActivate()
        {
            Player player = Main.LocalPlayer;

            if (Main.gameMenu)
            {
                return;
            }

            MagicPlayerData data = player.GetModPlayer<MagicPlayerData>();
            _selectedElements = data.LearnedElements;
            _selectedPath = data.PureElemental;

            UIPanel elementFrame = new UIPanel();
            elementFrame.Width.Pixels = 85f;
            elementFrame.Height.Percent = 1f;

            _panel.Append(elementFrame);

            _elements.Height.Percent = 1;
            _elements.Width.Percent = 1f;
            _elements.Clear();

            foreach (var element in ElementLoader.LoadedContent)
            {
                UIPanel panel = new UIPanel();
                panel.Width.Pixels = 68;
                panel.Height.Pixels = 68;
                panel.OnLeftClick += (evt, el) => SelectElement(evt, el, element);
                if (_selectedElements != null && _selectedElements.Contains(element))
                {
                    panel.BorderColor = Color.Gold;
                    panel.BackgroundColor = Color.Navy;
                }
                else
                {
                    panel.BorderColor = Color.Black;
                    panel.BackgroundColor = DesaturatedBlue;
                }

                Texture2D tex = element.Icon.Value;
                UIImage image = new(tex)
                {
                    VAlign = 0.5f,
                    HAlign = 0.5f
                };
                image.Width.Pixels = 34;
                image.Height.Pixels = 34;

                panel.Append(image);
                _elements.Add(panel);
            }
            elementFrame.Append(_elements);
        }

        private void SelectElement(UIMouseEvent evt, UIElement listeningElement, Element element)
        {
            Player player = Main.LocalPlayer;
            MagicPlayerData data = player.GetModPlayer<MagicPlayerData>();
            if (listeningElement is UIPanel panel)
            {
                panel.BorderColor = Color.Gold;
                panel.BackgroundColor = Color.Navy;
            }
            if (Spellbound.debug)
            {
                data.LearnedElements.Clear();
                data.LearnedElements.Add(element);
            }
            //TODO: Sync it
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            _panel.RemoveChild(_elements);
        }
    }
}
