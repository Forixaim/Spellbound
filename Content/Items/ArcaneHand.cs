using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spellbound.Content.Items
{
    public class ArcaneHand : CastingItem
    {
        public override void SetDefaults()
        {
            FociVelocityMultiplier = 1.2f;
            base.SetDefaults();
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.mana = 12;
            Item.damage = 450;
            Item.width = 8;
            Item.height = 8;
            Item.noUseGraphic = true;
        }
    }
}
