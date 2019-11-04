using System.Collections.Generic;
using System.Linq;
using AvorilConsole.Entities.EntityInterfaces;
using AvorilConsole.Entities.Factory;

namespace AvorilConsole.Entities
{
    public abstract class BaseEntity : Entity, ILifeEntity,IAttackEntity, IDissectableEntity // Сущность является игровой и имеет показатели здоровья
    {
        public BaseEntity(EntityFactory.BaseEntityPattern _BaseEntityPattern)
        {
            // ILifeEntity
            EntityLifeState = _BaseEntityPattern.LifeEntity.EntityLifeState;
            MaxHealth = _BaseEntityPattern.LifeEntity.MaxHealth;
            Health = _BaseEntityPattern.LifeEntity.Health;

            // IAttackEntity
            HitDamage = _BaseEntityPattern.AttackEntity.HitDamage;
            NextAttack = _BaseEntityPattern.AttackEntity.NextAttack;
            BaseEffects = _BaseEntityPattern.AttackEntity.BaseEffects;
            EmmitableEffects = _BaseEntityPattern.AttackEntity.EmmitableEffects;
            IsStunned = _BaseEntityPattern.AttackEntity.IsStunned;
        }

        // Вызывается при совершении "хода"
        public virtual void WorldTurn()
        {
            
        }

        public virtual void BattleTurn()
        {
            // Invoke all active EmmitableEffects
            for(int i = 0; i < EmmitableEffects.Count; i++)
            {
                var eff = EmmitableEffects[i];
                eff.Emmit(this);
            }
            // Remove all deactivated EmmitableEffects 
            EmmitableEffects.RemoveAll(x => x.Steps <= 0);
        }

        #region ILifeEntity
        //  Состояние жизни сущности
        public LifeState EntityLifeState { get; private set; }

        // Максимальное здоровье сущности
        public int MaxHealth
        {
            get => maxHealth;
            private set => maxHealth = value;
        }
        // Здоровье сущности
        public int Health
        {
            get => health;
            // Если здоровье равно нулю или меньше нуля, то возвращаем ноль, а если здоровье больше или равно макс.здовровья, то возвращаем макс.здоровье
            private set => health = (value > 0) ? (value < MaxHealth) ? value : MaxHealth : 0;
        }
        

        private int maxHealth;
        private int health;


        // Метод лечения
        public void Heal(int _HealCount)
        {
            Health += _HealCount;
        }

        // Метод получения урона
        public void Damage(int _DamageCount)
        {
            Health -= _DamageCount;

            if(Health == 0)
            {
                EntityLifeState = LifeState.Dead;
            }
        }
        #endregion

        #region IDissectableEntity
        public DissectionState DissectionState { get; protected set; }
   
        // Должен возвращать новый инвентарь с предметами
        public abstract void Dissetc();
        #endregion

        #region IAttackEntity
        public AttackStructure NextAttack { get; protected set; }

        public List<CastableEffect> BaseEffects { get; protected set; }

        public List<EmmitableEffect> EmmitableEffects { get; protected set; }

        public bool IsStunned { get; protected set; }

        public int HitDamage { get; protected set; }

        public virtual Couple<AttackStructure, AttackInfo> DoAttack()
        {
            Couple<AttackStructure, AttackInfo> couple 
                = new Couple<AttackStructure, AttackInfo>(NextAttack, new AttackInfo(HitDamage));

            return couple;
        }

        public List<EmmitableEffect> GetEffects(string cas, Couple<AttackInfo, ILifeEntity> _Myself, AttackInfo _Oneself)
        {
            var emmits = new List<EmmitableEffect>();

            for (int i = 0; i < BaseEffects.Count; i++)
            {
                var castableEffect = BaseEffects[i];
                if(castableEffect.Key.Equals(cas) == true)
                {
                    var emmitableEffect = castableEffect.Cast(_Myself, _Oneself);
                    emmits.Add(emmitableEffect);
                }
            }

            // ADD INVENTORY EFFECTS
            throw new System.NotImplementedException();

            return emmits;
        }

        public void AddEffects(List<EmmitableEffect> _Effects)
        {
            if(_Effects != null || _Effects.Count > 0)
                EmmitableEffects.AddRange(_Effects);
        }

        public void ComputeDamage(AttackInfo _EnemyAttackInfo)
        {
            Damage(_EnemyAttackInfo.DamageCount);
        }
        #endregion
    }

}
