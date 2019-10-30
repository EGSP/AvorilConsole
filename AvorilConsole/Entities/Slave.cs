using AvorilConsole.Entities.Factory;
using AvorilConsole.Entities.EntityInterfaces;

namespace AvorilConsole.Entities
{
    public class Slave : BaseEntity
    {
        public Slave(EntityFactory.BaseEntityPattern _BaseEntityPattern) : base(_BaseEntityPattern)
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
