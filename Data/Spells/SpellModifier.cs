using Terraria.ModLoader;

namespace Spellbound.Data.Spells;

public abstract class SpellModifier : CoreContent
{
    public float Minimum { get; protected set; }
    public float Maximum { get; protected set; }
    public override void Load(Mod mod)
    {
        Name = GetType().Name;
        FullName = $"{mod.Name}/SpellModifier/{Name}";
    }

    public override void Unload()
    {

    }

    /// <summary>
    /// Handles AI logic for the specified projectile using the provided value.
    /// </summary>
    /// <param name="projectile">The projectile instance whose AI behavior is to be processed. Cannot be null.</param>
    /// <param name="value">A floating-point value that influences the AI logic applied to the projectile. The meaning of this value depends
    /// on the specific implementation.</param>
    public virtual void HandleAi(ModProjectile projectile, float value) {}
    public virtual void Handle
}