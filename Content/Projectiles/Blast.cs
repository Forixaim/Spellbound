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
            Player player = Main.player[Projectile.owner];
            MagicPlayerData modPlayer = player.GetModPlayer<MagicPlayerData>();



            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (!other.active) continue;
                if (other.whoAmI == Projectile.whoAmI) continue; // skip self
                if (other.owner == Projectile.owner) continue;  // optional: skip same owner
                if (other.friendly == Projectile.friendly) continue; // skip same team

                if (Projectile.Hitbox.Intersects(other.Hitbox))
                {
                    Projectile.Kill();
                    other.Kill();
                    return;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }

        public override void OnSpawn(IEntitySource source)
        {
            
        }
    }
}
