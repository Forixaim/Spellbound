using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spellbound.Data.Spells;

namespace Spellbound.Loaders
{
    internal class SpellModifierLoader
    {
        public static readonly List<SpellModifier> Modifiers = new();

        /// <summary>
        /// Legacy only, this is used to get the modifier from the ID, which is used in the spell modifier system. This is not used for anything else.
        /// </summary>
        /// <param name="id">Because that the id is literally just the index of the modifier in the list adding new modifiers down the line will change the id of all modifiers after it, so this is not a good system.</param>
        /// <returns></returns>
        public static SpellModifier GetFromId(int id)
        {
            return Modifiers[id];
        }
        /// <summary>
        /// The intended way to get the modifier, this is used in the spell modifier system and is the recommended way to get the modifier. Primarily for serialization and deserialization of the modifiers, as well as for modders who want to get the modifier from the name instead of the id, which is more stable. The name is the full name of the modifier, which is the namespace + class name. For example, if you have a modifier called "MyModifier" in a namespace called "MyMod", the full name would be "MyMod.MyModifier". 
        /// </summary>
        /// <param name="name">Primarily only for serialization and deserialization. Because the name is more stable than the id, as adding new modifiers down the line will not change the name of existing modifiers, but it will change the id of existing modifiers.
        /// </param>
        /// <returns></returns>
        public static SpellModifier GetFromName(string name)
        {
            foreach (var t in Modifiers)
            {
                if (t.FullName == name)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// A more programmer-friendly way to get the modifier. Returns the actual type of the modifier, so you can use it to get the modifier from the type instead of the name or id. This is useful for modders who want to get the modifier from the type, which is more stable than the name or id. This is also useful for serialization and deserialization of the modifiers, as you can use the type to get the modifier instead of the name.
        /// </summary>
        /// <typeparam name="TC"> The most stable way for modders to get the modifier, but since the type itself cannot be serialized or deserialized, this is not used for that. This is primarily for modders who want to get the modifier from the type, which is more stable than the name or id. This is also useful for serialization and deserialization of the modifiers, as you can use the type to get the modifier instead of the name improving intuition.
        /// </typeparam>
        /// <returns></returns>
        public static SpellModifier GetFromType<TC>() where TC : SpellModifier
        {
            foreach (var t in Modifiers)
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
