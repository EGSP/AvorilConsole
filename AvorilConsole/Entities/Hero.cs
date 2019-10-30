using AvorilConsole.Entities.EntityInterfaces;
using AvorilConsole.Entities.Factory;

namespace AvorilConsole.Entities
{
    public class Hero : BaseEntity 
    {
        public Hero(EntityFactory.BaseEntityPattern _BaseEntityPattern) : base(_BaseEntityPattern)
        {


        }
        

        public override void Dissetc()
        {
            throw new System.NotImplementedException();
        }

        public override Couple<AttackStructure, AttackInfo> DoAttack()
        {
            throw new System.NotImplementedException();
        }
    }

}
