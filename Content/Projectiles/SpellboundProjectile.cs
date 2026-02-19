using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellbound.Data;
using Spellbound.Data.Spells;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellbound.Content.Projectiles
{
    public abstract class SpellboundProjectile : ModProjectile
    {
        public SpellInstance spellData;

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            MagicPlayerData modPlayer = player.GetModPlayer<MagicPlayerData>();
            Color projColor = spellData.BoundElement.MagicColor;
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() / 2f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            for (int i = 0; i < 3; i++)
            {
                float scale = Projectile.scale * (1.2f + i * 0.15f);
                Color glow = projColor * 0.35f;

                Main.spriteBatch.Draw(tex, pos, null, glow,
                    Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );
            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                projColor,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            
            base.ModifyDamageHitbox(ref hitbox);
        }

        public void initializeData(SpellInstance data)
        {
            spellData = data;
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
            
        }
    }
}
