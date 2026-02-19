using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Spellbound.Data
{
    public abstract class CoreContent : IModType, ILoadable
    {
        public int TypeID { get; internal set; }

        public virtual void Load(Mod mod)
        {
            Mod = mod;
            Name = GetType().Name;
            FullName = $"{mod.Name}/{Name}";
        }

        public virtual void Unload()
        {

        }

        public Mod Mod { get; private set; }
        public string Name { get; protected set; }
        public string FullName { get; protected set; }
    }
}
