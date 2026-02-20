using Spellbound.Loaders;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Spellbound.Content.Elements;
using Spellbound.Content.Items;
using Spellbound.Content.Spells;
using Spellbound.Data.Magic;
using Spellbound.Data.Spells;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellbound.Data
{
    public class MagicPlayerData : ModPlayer
    {
        public List<Element> LearnedElements = new();
        public bool PureElemental = false;
        public bool FirstTime = true;
        public List<SpellType> LearnedSpells = new();
        public override bool IsCloneable => false;
        public override void Initialize()
        {
            base.Initialize();
            if (Spellbound.debug && LearnedElements.Count == 0)
            {
                LearnedElements.Add(ElementLoader.getFromType<LightElement>());
            }
            LearnedSpells.Add(SpellLoader.GetFromType<Blast>());
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
            Item item = new Item(ModContent.ItemType<ArcaneHand>());
            item.Prefix(PrefixID.Mythical);
            yield return item;
        }

        public override void SaveData(TagCompound tag)
        {
            if (tag != null)
            {
                List<string> spellIds = new();
                List<string> elementIds = new();
                foreach (Element element in LearnedElements)
                {
                    if (element != null)
                    {
                        elementIds.Add(element.FullName ?? "");
                    }
                }
                foreach (SpellType spell in LearnedSpells)
                {
                    spellIds.Add(spell.FullName ?? "");
                }
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
            if (tag.ContainsKey("spellbound:learnedElements"))
            {
                List<string> elementIds = tag.Get<List<string>>("spellbound:learnedElements");
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
                List<string> spellIds = tag.Get<List<string>>("spellbound:learnedSpells");
                foreach (string id in spellIds)
                {
                    if (id == "") continue;
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
