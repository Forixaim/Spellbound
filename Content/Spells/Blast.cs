using Spellbound.Data.Spells;
using Terraria;
using Terraria.ModLoader;

namespace Spellbound.Content.Spells
{
    /**
     * In brief, a simple spell but with the amount of modifiers one can put on this spell...
     * The blast is in a simple term, rather simple but with the right 
     */
    internal class Blast : ProjectileSpellType
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            baseVelocity = 10f;
            ProjectileID = ModContent.ProjectileType<Projectiles.Blast>();
        }
    }
}
