using Spellbound.Data;
using Spellbound.Data.Magic;
using Spellbound.Loaders;
using Terraria;
using Terraria.ModLoader;

namespace Spellbound.Data.Spells
{
    public abstract class SpellType : CoreContent
    {
        public float baseDamage { get; protected set; }
        public float baseManaCost { get; protected set; }
        public float baseCrit { get; protected set; }
        public float baseAP { get; protected set; }
        public float baseAttackSpeed { get; protected set; }
        public float baseVelocity { get; protected set; }
        public float basePenetration { get; protected set; }

        public virtual void SetDefaults()
        {

        }

        public virtual float CalculateComplexity()
        {
            return 0;
        }
        public sealed override void Load(Mod mod)
        {
            SetDefaults();
            Name = GetType().Name;
            mod.Logger.Debug($"Loaded spell: {Name}");
            FullName = $"{mod.Name}/Spell/{Name}";
            TypeID = SpellLoader.LoadedContent.Count;
            SpellLoader.LoadedContent.Add(this);
        }

        public sealed override void Unload()
        {
            base.Unload();
        }

        public abstract void Cast(Player player, MagicPlayerData data);
    }
}
