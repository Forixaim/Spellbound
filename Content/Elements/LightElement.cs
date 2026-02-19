using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Spellbound.Data.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellbound.Content.Elements
{
    internal class LightElement : Element
    {
        public override void SetDefaults()
        {
            ProjectileSpeedMultiplier = 1.85f;
            KnockbackMultiplier = 1f;
            DamageMultiplier = 1f;
            ManaCostMultiplier = 0.95f;
            CastSpeedMultiplier = 1.1f;
            CritChanceBonus = 4f;
            ArmorPenetrationBonus = 0;
            AssociatedEffect = BuffID.Blackout;
            MagicColor = new Color(255, 255, 200);
        }

        public override void handleProjectileDust(ModProjectile proj)
        {
            Dust dust = Dust.NewDustDirect(proj.Projectile.position, proj.Projectile.width, proj.Projectile.height, DustID.GoldFlame,  0f, 0f,  0, default, proj.Projectile.scale);
        }

        public override void handleKill(ModProjectile proj)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(proj.Projectile.position, proj.Projectile.width, proj.Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 3.5f);
                dust.velocity *= 1.4f;
            }
        }
    }
}
