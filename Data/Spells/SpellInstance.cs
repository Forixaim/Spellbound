using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spellbound.Data.Magic;
using Spellbound.Loaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Spellbound.Data.Spells
{
    public class SpellInstance
    {
        public Element BoundElement { get; private set; }
        public SpellType type { get; private set; }
        public Dictionary<string, float> Properties { get; } = new();
        public List<SpellModifier> Modifiers { get; } = new();

        public TagCompound Serialize()
        {
            TagCompound tag = new TagCompound();
            tag["Type"] = type.FullName;
            tag["Properties"] = Properties;
            tag["BoundElement"] = BoundElement?.FullName ?? "";
            return tag;
        }

        public static SpellInstance Deserialize(TagCompound tag)
        {
            SpellInstance instance = new SpellInstance();
            Dictionary<string, float> properties = tag.Get<Dictionary<string, float>>("Properties");
            foreach (var name in properties)
            {
                instance.AddModifier(SpellModifierLoader.GetFromName(name.Key), name.Value);
            }
            string elementName = tag.GetString("BoundElement");
            if (!string.IsNullOrEmpty(elementName))
            {
                instance.BoundElement = ElementLoader.getFromName(elementName);
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
            Modifiers.Add(modifier);
            Properties.Add(modifier.FullName, value);
        }

        public void HandleSend(BinaryWriter writer)
        {
            writer.Write(Spellbound.NetVer);
            writer.Write(type.FullName ?? "");
            writer.Write(BoundElement.FullName ?? "");

            writer.Write7BitEncodedInt(Properties.Count);
            
            foreach (var kvp in Properties)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
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
                type = SpellLoader.GetFromName(reader.ReadString()),
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
