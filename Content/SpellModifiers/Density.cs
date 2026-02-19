using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spellbound.Data.Spells;

namespace Spellbound.Content.SpellModifiers
{
    internal class Density : SpellModifier
    {
        public override void Load(Terraria.ModLoader.Mod mod)
        {
            base.Load(mod);
            Minimum = 0.0f;
            Maximum = 1f;
        }
    }
}
