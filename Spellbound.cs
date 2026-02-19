using Microsoft.Xna.Framework.Graphics;
using Spellbound.Data.Magic;
using Spellbound.Data.Spells;
using Spellbound.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Spellbound
{
	public class Spellbound : Mod
    {
        public static byte NetVer = 1;
        public static ModKeybind DebugKeybind;

        public override void Load()
        {
            base.Load();
            DebugKeybind = KeybindLoader.RegisterKeybind(this, "Debug Key", "P");
        }

        public override void Unload()
        {
            base.Unload();
            DebugKeybind = null;
        }
    }
}
