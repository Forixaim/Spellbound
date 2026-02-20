using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Spellbound.Content.Spells;
using Spellbound.Data;
using Spellbound.Data.Spells;
using Spellbound.Loaders;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellbound.Content.Items
{ 
	public abstract class CastingItem : ModItem
	{

		public float FociVelocityMultiplier { get; protected set; }
		public float ManaCostMultiplier { get; protected set; }
		public float DamageMultiplier { get; protected set; }
		public float KnockbackMultiplier { get; protected set; }
		public string CurrentPlayer;
        public int equippedSpell { get; protected set; } = 0;
		public readonly List<SpellInstance> StoredSpells = new();

		public override void SaveData(TagCompound tag)
		{
			tag["spellbound:equippedSpells"] = equippedSpell;
			List<TagCompound> storedSpellsTag = new();
            foreach (var spell in StoredSpells)
            {
				storedSpellsTag.Add(spell.Serialize());
            }
			tag["spellbound:storedSpells"] = storedSpellsTag;
		}

        public override void LoadData(TagCompound tag)
        { 
			if (tag.ContainsKey("spellbound:equippedSpells"))
                equippedSpell = tag.GetInt("spellbound:equippedSpells");
            if (tag.ContainsKey("spellbound:storedSpells"))
            {
                List <TagCompound> storedSpellsTag = tag.Get<List<TagCompound>>("spellbound:storedSpells");
                foreach (var tags in storedSpellsTag)
                {
                    StoredSpells.Add(SpellInstance.Deserialize(tags));
                }
            }
            
        }


        public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
            
        }

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 1f;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.UseSound = SoundID.DD2_BookStaffCast;
			Item.useTurn = true;
			Item.knockBack = 4f;
		}

		public override bool AltFunctionUse(Player player)
		{
			//TODO: Cycle through stored spells
			return base.AltFunctionUse(player);
		}

		public override void RightClick(Player player)
		{
			//:TODO: Open spell placement UI
			//Create the list if it doesn't exist

            if (StoredSpells.Count == 0)
            {
                if (Spellbound.debug && (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.MultiplayerClient))
                {
					
					Main.NewText("No spells stored, adding debug spell");
					SpellInstance test = new SpellInstance(player.GetModPlayer<MagicPlayerData>().LearnedElements[0], SpellLoader.GetFromType<Blast>(), new Dictionary<string, float>());
                    if (test.Type == null)
                    {
						Main.NewText("There is no spell...");
                        return;
                    }
                    Main.NewText("Spell created named: " + test.Type.FullName + ". bound to Element: " + test.BoundElement.Name);
					StoredSpells.Add(test);
                }
            }
			base.RightClick(player);
		}

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			int mana = Item.mana;
			// int spellManaCost = equippedSpellType != null ? (int)equippedSpellType.baseManaCost : 0;
			// //get the difference to set it to the spell mana cost, then apply the multiplier
			// int difference = spellManaCost - mana;
			// reduce -= difference;

			base.ModifyManaCost(player, ref reduce, ref mult);
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{

			//damage.Base = equippedSpellType.baseDamage;
			//StatModifier multiplier = new StatModifier(0, equippedSpellType.boundElement.DamageMultiplier);
			//damage.CombineWith(multiplier);
		}

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage,
			ref float knockback)
		{
			MagicPlayerData data = player.GetModPlayer<MagicPlayerData>();

			int index = equippedSpell;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
	}
}
