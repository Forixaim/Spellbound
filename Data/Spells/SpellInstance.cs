using System.Collections.Generic;
using System.IO;
using Spellbound.Content.Items;
using Spellbound.Data.Magic;
using Spellbound.Loaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellbound.Data.Spells
{
    public class SpellInstance
    {
        public Element BoundElement { get; private set; }
        public SpellType Type { get; set; }
        public Dictionary<string, float> Properties { get; } = new();
        public List<SpellModifier> Modifiers { get; } = new();

        public SpellInstance(Element boundElement, SpellType type, Dictionary<string, float> properties)
        {
            BoundElement = boundElement;
            Type = type;
            foreach (var kvp in properties)
            {
                Properties.Add(kvp.Key, kvp.Value);
            }
            Modifiers.AddRange(BuildModifiers());
        }

        public float calculateTotalManaCost(CastingItem item)
        {
            return 0;
        }

        private SpellInstance() { }

        public List<SpellModifier> BuildModifiers()
        {
            List<SpellModifier> modifiers = new();
            foreach (var kvp in Properties)
            {
                var modifier = SpellModifierLoader.GetFromName(kvp.Key);
                if (modifier != null)
                {
                    modifiers.Add(modifier);
                }
            }
            return modifiers;
        }

        public TagCompound Serialize()
        {
            TagCompound tag = new TagCompound
            {
                ["Type"] = Type?.FullName ?? "",
                ["BoundElement"] = BoundElement?.FullName ?? ""

            };
            TagCompound propertiesTag = new TagCompound();
            if (Properties != null && Properties.Count > 0)
            {
                foreach (var kvp in Properties)
                {
                    if (!string.IsNullOrEmpty(kvp.Key))
                    {
                        propertiesTag[kvp.Key] = kvp.Value;
                    }
                }
            }
            tag["Properties"] = propertiesTag;
            return tag;
        }

        public static SpellInstance Deserialize(TagCompound tag)
        {
            SpellInstance instance = new SpellInstance();

            string typeName = tag.GetString("Type");
            if (!string.IsNullOrEmpty(typeName))
            {
                instance.Type = SpellLoader.GetFromName(typeName);
            }

            string elementName = tag.GetString("BoundElement");
            if (!string.IsNullOrEmpty(elementName))
            {
                instance.BoundElement = ElementLoader.getFromName(elementName);
            }

            TagCompound propertiesTag = tag.GetCompound("Properties");
            foreach (var kvp1 in propertiesTag)
            {
                var modifier = SpellModifierLoader.GetFromName(kvp1.Key);
                if (modifier != null) 
                {
                    instance.AddModifier(modifier, propertiesTag.GetFloat(kvp1.Key));
                }
            }

            return instance;
        }

        public void HandleAi(ModProjectile projectile)
        {
            foreach (SpellModifier modifier in Modifiers)
            {
                float value = Properties[modifier.FullName];
                modifier.HandleAi(projectile, value);
            }
        }

        public void AddModifier(SpellModifier modifier, float value)
        {
            // Check if modifier is null first, then check FullName
            if (modifier == null || string.IsNullOrEmpty(modifier.FullName))
            {
                return;
            }
            Modifiers.Add(modifier);
            Properties.Add(modifier.FullName, value);
        }

        public void HandleSend(BinaryWriter writer)
        {
            writer.Write(Spellbound.NetVer);
            writer.Write(Type?.FullName ?? "");
            writer.Write(BoundElement?.FullName ?? "");

            writer.Write7BitEncodedInt(Properties.Count);

            foreach (var kvp in Properties)
            {
                // Ensure we don't send null keys
                if (!string.IsNullOrEmpty(kvp.Key))
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }
            }
        }

        public void HandleStats(ModProjectile projectile)
        {
            foreach (SpellModifier modifier in Modifiers)
            {
                float value = Properties[modifier.FullName];
                modifier.HandleStats(projectile, value);
            }
        }

        public static SpellInstance HandleReceive(BinaryReader reader)
        {
            byte NetVer = reader.ReadByte();

            var s = new SpellInstance()
            {
                Type = SpellLoader.GetFromName(reader.ReadString()),
                BoundElement = ElementLoader.getFromName(reader.ReadString()),
            };

            int count = reader.Read7BitEncodedInt();

            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                float value = reader.ReadSingle();
                s.AddModifier(SpellModifierLoader.GetFromName(key), value);
            }

            return s;
        }
    }
}
