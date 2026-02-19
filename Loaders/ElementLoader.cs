using System;
using System.Collections.Generic;
using System.Reflection;
using Spellbound.Data.Magic;
using Terraria.ModLoader;

namespace Spellbound.Loaders
{
    public class ElementLoader
    {
        public static readonly List<Element> LoadedContent = new();

        public static Element getFromID(int id)
        {
            return LoadedContent[id];
        }

        public static Element getFromName(string name)
        {
            foreach (var t in LoadedContent)
            {
                if (t.FullName == name)
                {
                    return t;
                }
            }
            return null;
        }

        public static Element getFromType<TC>() where TC : Element
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
