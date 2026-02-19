using Spellbound.Data.Magic;
using Spellbound.Loaders;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Spellbound.Content.Items;
using Spellbound.Data.Spells;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellbound.Data
{
    public class MagicPlayerData : ModPlayer
    {
        public List<Element> LearnedElements = new();
        public bool PureElemental = false;
        public bool FirstTime = true;
        public string PlayerGuid = string.Empty;
        public List<SpellType> LearnedSpells = new();
        public override bool IsCloneable => false;
        public override void Initialize()
        {
            base.Initialize();
            PlayerGuid = Guid.NewGuid().ToString("N");
        }

        public override void OnEnterWorld()
        {
            if (FirstTime)
            {
                if (Random.Shared.Next(0, 4) <= 1)
                {
                    PureElemental = true;
                    Main.NewText("You feel something is off...", Color.Cyan);
                }
                FirstTime = false;

            }
            base.OnEnterWorld();
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            yield return new Item(ModContent.ItemType<ArcaneHand>());
        }

        public override void SaveData(TagCompound tag)
        {
            if (tag != null)
            {
                string[] spellIds = new string[LearnedSpells.Count];
                string[] elementIds = new string[LearnedElements.Count];
                foreach (Element element in LearnedElements)
                {
                    elementIds[LearnedElements.IndexOf(element)] = element.FullName;
                }
                foreach (SpellType spell in LearnedSpells)
                {
                    spellIds[LearnedSpells.IndexOf(spell)] = spell.FullName;
                }
                tag.Add("spellbound:playerGuid", PlayerGuid);
                tag.Add("spellbound:pureElemental", PureElemental);
                tag.Add("spellBound:firstTime", FirstTime);
                tag.Add("spellbound:learnedElements", elementIds);
                tag.Add("spellbound:learnedSpells", spellIds);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("spellBound:firstTime"))
            {
                FirstTime = tag.GetBool("spellBound:firstTime");
            }
            if (tag.ContainsKey("spellbound:pureElemental"))
            {
                PureElemental = tag.GetBool("spellbound:pureElemental");
            }
            if (tag.ContainsKey("spellbound:playerGuid"))
            {
                PlayerGuid = tag.GetString("spellbound:playerGuid");
            }
            else
            {
                PlayerGuid = Guid.NewGuid().ToString("N");
            }
            if (tag.ContainsKey("spellbound:learnedElements"))
            {
                string[] elementIds = tag.Get<List<string>>("spellbound:learnedElements").ToArray();
                foreach (string id in elementIds)
                {
                    Element element = ElementLoader.getFromName(id);
                    if (element != null)
                    {
                        LearnedElements.Add(element);
                    }
                }
            }
            if (tag.ContainsKey("spellbound:learnedSpells"))
            {
                string[] spellIds = tag.Get<List<string>>("spellbound:learnedSpells").ToArray();
                foreach (string id in spellIds)
                {
                    SpellType spell = SpellLoader.GetFromName(id);
                    if (spell != null)
                    {
                        LearnedSpells.Add(spell);
                    }
                }
            }
        }

    }
}
