using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellbound.Loaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace Spellbound.Data.Magic
{
    public abstract class Element : CoreContent
    {
        public float DamageMultiplier { get; protected set; }
        public float ProjectileSpeedMultiplier { get; protected set; }
        public float CastSpeedMultiplier { get; protected set; }
        public float ManaCostMultiplier { get; protected set; }
        public int ArmorPenetrationBonus { get; protected set; }
        public float CritChanceBonus { get; protected set; }
        public float KnockbackMultiplier { get; protected set; }
        public int AssociatedEffect { get; protected set; }
        public Color MagicColor { get; protected set; }
        public Texture2D Icon { get; protected set; }


        public virtual void SetDefaults()
        {
            DamageMultiplier = 1f;
            ProjectileSpeedMultiplier = 1f;
            CastSpeedMultiplier = 1f;
            ManaCostMultiplier = 1f;
            CritChanceBonus = 1f;
            AssociatedEffect = BuffID.OnFire;
            ArmorPenetrationBonus = 0;
            MagicColor = Color.White;
        }

        protected virtual string TexturePath()
        {
            
            string path = GetType().FullName;
            if (path == null)
            {
                return "";
            }

            path = path.Replace('.', '/');

            return path;
        }

        public override void Load(Mod mod)
        {
            SetDefaults();
            Name = GetType().Name;
            FullName = $"{mod.Name}/Element/{Name}";
            Texture2D buffer;
            try
            {
                buffer = ModContent.Request<Texture2D>(TexturePath()).Value;
            }
            catch
            {
                mod.Logger.Warn($"Could not find texture for magic: {Name} at path: {TexturePath()}");
                buffer = ModContent.Request<Texture2D>(ModContent.ItemType<UnloadedItem>().GetType().FullName).Value;
            }
            Icon = buffer;
            TypeID = ElementLoader.LoadedContent.Count;
            mod.Logger.Debug($"Loaded magic: {Name}, with ID: {TypeID}");
            ElementLoader.LoadedContent.Add(this);
        }

        public virtual void handleProjectileDust(ModProjectile proj)
        {

        }

        public virtual void handleKill(ModProjectile proj)
        {

        }
    }
}
