using Godot;
using Scalepact.Utilities;

namespace Scalepact.DamageSystem
{
    public partial class AreaDamager : Damager
    {
        [Export] int areaDamage;

        public override void _Ready()
        {
            SetupAreaDamager();
        }

        public void SetupAreaDamager()
        {
            damageAmount = areaDamage;
        }
    }
}