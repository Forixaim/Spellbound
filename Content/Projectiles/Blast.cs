using Spellbound.Data;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellbound.Content.Projectiles
{
    public class Blast : SpellboundProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 2f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            AIType = ProjectileID.Bullet;
            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 0;
            Projectile.timeLeft = 600;
        }


        public override void AI()
        {
            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
    }
}
