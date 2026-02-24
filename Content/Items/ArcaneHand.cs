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
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.damage = 13;
            Item.width = 8;
            Item.height = 8;
            Item.noUseGraphic = true;
        }
    }
}
