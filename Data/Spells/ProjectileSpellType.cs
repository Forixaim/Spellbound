using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Spellbound.Data.Spells
{
    internal abstract class ProjectileSpellType : SpellType
    {
        public int ProjectileID { get; protected set; }


        public override void Cast(Player player, MagicPlayerData data)
        {
        }
    }
}
