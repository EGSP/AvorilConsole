using AvorilConsole.Items;

namespace AvorilConsole
{
    namespace Entities.EntityInterfaces
    {
        public enum LifeState
        {
            Alive,
            Dead
        }

        public enum DissectionState
        {
            NotDissected, // Не расчленён
            Dissected // Расчленён
        }

        // Сущность, которую можно расчленить
        public interface IDissectableEntity
        {
            DissectionState DissectionState { get; }

            void Dissetc();
        }

        public interface ILifeEntity // Сущность имеет показатели здоровья 
        {
            LifeState EntityLifeState { get; }

            int MaxHealth { get; }
            int Health { get; }

            void Heal(int _HealCount);
            void Damage(int _DamageCount);
        }
    }
}
