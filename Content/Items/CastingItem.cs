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
		public Dictionary<string, int> equippedSpell { get; protected set; }
		public readonly Dictionary<string, List<SpellInstance>> StoredSpells = new();

		public override void SaveData(TagCompound tag)
		{
			Dictionary<string, int> equippedSpells = new();
			foreach (var kvp in equippedSpell)
			{
				equippedSpells[kvp.Key] = kvp.Value;
			}
			tag["spellbound:equippedSpells"] = equippedSpells;
			Dictionary<string, List<TagCompound>> storedSpells = new();
			foreach (var kvp in StoredSpells)
			{
				List<TagCompound> spellList = new();
				foreach (var spell in kvp.Value)
				{
					spellList.Add(spell.Serialize());
				}
				storedSpells[kvp.Key] = spellList;
			}
			tag["spellbound:storedSpells"] = storedSpells;
			base.SaveData(tag);
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			Dictionary<string, int> equippedSpellsBuffer;
			Dictionary<string, List<TagCompound>> storedSpellsBuffer;
			if (tag.TryGet("spellbound:equippedSpells", out equippedSpellsBuffer))
			{
				if (equippedSpellsBuffer.Count > 0)
				{
					foreach (var kvp in equippedSpellsBuffer)
					{
						equippedSpell[kvp.Key] = kvp.Value;
                    }
				}
			}

			if (tag.TryGet("spellbound:storedSpells", out storedSpellsBuffer))
			{
				if (storedSpellsBuffer.Count > 0)
				{
					foreach (var kvp in storedSpellsBuffer)
					{
						List<SpellInstance> instanceBuffer = new();
						foreach (var spellTag in kvp.Value)
						{
							instanceBuffer.Add(SpellInstance.Deserialize(spellTag));
						}
						StoredSpells[kvp.Key] = instanceBuffer;
					}
				}
			}
		}

        //Netcode is used for multiplayer syncing, so we need to sync the equipped spell index and its properties
        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
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
			List<SpellInstance> playerSpells = StoredSpells.GetValueOrDefault(player.GetModPlayer<MagicPlayerData>().PlayerGuid, new List<SpellInstance>());
            int index = equippedSpell.GetValueOrDefault(player.GetModPlayer<MagicPlayerData>().PlayerGuid, 0);
            if (playerSpells.Count > 0)
            {
				index = (index + 1) % playerSpells.Count;
				equippedSpell[player.GetModPlayer<MagicPlayerData>().PlayerGuid] = index;
            }
            return base.AltFunctionUse(player);
        }

        public override void RightClick(Player player)
        {
            //:TODO: Open spell placement UI
            //Create the list if it doesn't exist
			if (!StoredSpells.ContainsKey(player.GetModPlayer<MagicPlayerData>().PlayerGuid))
			{
				StoredSpells[player.GetModPlayer<MagicPlayerData>().PlayerGuid] = new List<SpellInstance>();
            }
            base.RightClick(player);
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            string uid = player.GetModPlayer<MagicPlayerData>().PlayerGuid;
			int mana = Item.mana;
			int spellManaCost = equippedSpellType != null ? (int)equippedSpellType.baseManaCost : 0;
			//get the difference to set it to the spell mana cost, then apply the multiplier
			int difference = spellManaCost - mana;
			reduce -= difference;

			base.ModifyManaCost(player, ref reduce, ref mult);
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
            string uid = player.GetModPlayer<MagicPlayerData>().PlayerGuid;

            damage.Base = equippedSpellType.baseDamage;
			StatModifier multiplier = new StatModifier(0, equippedSpellType.boundElement.DamageMultiplier);
			damage.CombineWith(multiplier);
		}

		public override void UpdateInventory(Player player)
		{
			CurrentPlayer = player;
			if (player !=)
		}

		//Internal methods
        private SpellInstance GetEquippedSpell(Player player)
        {
            string uid = player.GetModPlayer<MagicPlayerData>().PlayerGuid;
			List<SpellInstance> playerSpells = StoredSpells.GetValueOrDefault(uid, new List<SpellInstance>());
			int index = equippedSpell.GetValueOrDefault(uid, 0);
            if (playerSpells.Count > 0 && index < playerSpells.Count)
            {
                return playerSpells[index];
            }
			return null;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage,
			ref float knockback)
		{
			MagicPlayerData data = player.GetModPlayer<MagicPlayerData>();

			if (equippedSpell[data.PlayerGuid]. is ProjectileSpellType pSpell)
			{
				
				type = pSpell.ProjectileID;
				if (velocity == Vector2.Zero)
				{
					return;
				}
				velocity.Normalize();
				velocity *= pSpell.baseVelocity * FociVelocityMultiplier * pSpell.boundElement.ProjectileSpeedMultiplier;

			}
		}
	}
}
