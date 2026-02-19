using Spellbound.Data.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellbound.Content.Elements
{
    public class FireElement : Element
    {

        public override void SetDefaults()
        {
            ProjectileSpeedMultiplier = 1.15f;
            DamageMultiplier = 1.25f;
            ManaCostMultiplier = 1.02f;
            CastSpeedMultiplier = 1.0f;
            CritChanceBonus = 4f;
            ArmorPenetrationBonus = 0;
            AssociatedEffect = BuffID.OnFire3;
            MagicColor = new Color(255, 120, 0);
        }

        public override void handleProjectileDust(ModProjectile proj)
        {
            Dust dust = Dust.NewDustDirect(proj.Projectile.position, proj.Projectile.width, proj.Projectile.height, DustID.Torch, 0f, 0f, 0, default, proj.Projectile.scale);
        }

        public override void handleKill(ModProjectile proj)
        {
            
        }
    }
}
