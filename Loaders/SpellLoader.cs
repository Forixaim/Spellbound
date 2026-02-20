using System;
using System.Collections.Generic;
using System.Reflection;
using Spellbound.Data.Spells;
using Terraria.ModLoader;

namespace Spellbound.Loaders
{
    public class SpellLoader
    {
        public static readonly List<SpellType> LoadedContent = new();

        public static SpellType GetFromID(int id)
        {
            return LoadedContent[id];
        }

        public static SpellType GetFromName(string FullName)
        {
            foreach (SpellType type in LoadedContent)
            {
                if (type.FullName == FullName)
                {
                    return type;
                }
            }
            return null;
        }

        public static SpellType GetFromType<TC>() where TC : SpellType
        {
            foreach (var t in LoadedContent)
            {
                if (t.GetType() == typeof(TC))
                {
                    return t;
                }
            }
            return null;
        }
    }
}
