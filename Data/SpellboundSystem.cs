using Microsoft.Xna.Framework;
using Spellbound.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellbound.Data
{ 
    /// <summary>
    /// This is to be removed and be put in a core mod called Combat Reforged: Core
    /// </summary>
    internal class SpellboundSystem : ModSystem
    {
        private GameTime _lastUpdateUiGameTime;

        internal UserInterface UserInterface;
        internal ElementSelectionUi ElementSelectionUi;
        public override void Load()
        {
            base.Load();
            if (!Main.dedServ)
            {
                HandleClientLoad();
            }
        }

        public override void Unload()
        {
            base.Unload();
            if (!Main.dedServ)
            {
                HandleClientUnload();
            }
        }

        private void HandleClientLoad()
        {
            UserInterface = new UserInterface();

            ElementSelectionUi = new ElementSelectionUi();
            ElementSelectionUi.Activate();
        }

        private void HandleClientUnload()
        {
            ElementSelectionUi.Unload();
            UserInterface = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (UserInterface?.CurrentState != null)
            {
                UserInterface.Update(gameTime);
            }

            if (Spellbound.DebugKeybind.JustPressed)
            {
                if (UserInterface?.CurrentState != null)
                {
                    HideMyUI();
                }
                else
                {
                    ShowMyUI();
                }
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "spellbound: element_selection_ui",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && UserInterface?.CurrentState != null)
                        {
                            UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        internal void ShowMyUI()
        {
            UserInterface?.SetState(ElementSelectionUi);
        }

        internal void HideMyUI()
        {
            UserInterface?.SetState(null);
        }
    }

    
}
