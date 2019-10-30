using System;
using System.Collections.Generic;
using System.Text;

using AvorilConsole.Entities.EntityInterfaces;

namespace AvorilConsole.Entities.Factory
{
    public static class EntityFactory
    {

        public static ILifeEntity CreateLifeEntity(LifeState _LifeState, int _MaxHealth, int _Health)
        {
            var lifeEntityPattern = new LifeEntityPattern(_LifeState, _MaxHealth, _Health);

            return lifeEntityPattern;
        }

        public static IAttackEntity CreateAttackEnitity(int _HitDamage,AttackStructure _NextAttack, List<CastableEffect> _BaseEffects, bool _IsStunned)
        {
           
            if (_BaseEffects == null)
                _BaseEffects = new List<CastableEffect>();

            var attackEntityPattern = new AttackEntityPattern(_HitDamage, _NextAttack, _BaseEffects, _IsStunned);

            return attackEntityPattern;
        }

        // ----------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------
        class LifeEntityPattern : ILifeEntity
        {
            public LifeEntityPattern(LifeState _LifeState, int _MaxHealth, int _Health)
            {
                EntityLifeState = _LifeState;

                MaxHealth = _MaxHealth;
                Health = _Health;
            }

            public LifeState EntityLifeState { get; set; }
            public int MaxHealth { get; set; }

            public int Health { get; set; }

            public void Damage(int _DamageCount)
            {
                throw new NotImplementedException();
            }

            public void Heal(int _HealCount)
            {
                throw new NotImplementedException();
            }
        }

        class AttackEntityPattern : IAttackEntity
        {
            public AttackEntityPattern(int _HitDamage,AttackStructure _NextAttack, List<CastableEffect> _BaseEffects, bool _IsStunned)
            {
                HitDamage = _HitDamage;
                NextAttack = _NextAttack;
                BaseEffects = _BaseEffects;
                IsStunned = _IsStunned;
            }

            public int HitDamage { get; protected set; }

            public AttackStructure NextAttack { get; protected set; }

            public List<CastableEffect> BaseEffects { get; protected set; }

            public List<EmmitableEffect> EmmitableEffects { get; protected set; }

            public bool IsStunned { get; protected set; }

            public void AddEffects(List<EmmitableEffect> _Effects)
            {
                throw new NotImplementedException();
            }

            public Couple<AttackStructure, AttackInfo> DoAttack()
            {
                throw new NotImplementedException();
            }

            public List<EmmitableEffect> GetEffects(string cas, Couple<AttackInfo, ILifeEntity> _Myself, AttackInfo _Oneself)
            {
                throw new NotImplementedException();
            }
        }

        public class BaseEntityPattern
        {
            public BaseEntityPattern(ILifeEntity _ILifeEntity, IAttackEntity _IAttackEntity)
            {
                LifeEntity = _ILifeEntity;
                AttackEntity = _IAttackEntity;
            }

            public ILifeEntity LifeEntity { get; }
            public IAttackEntity AttackEntity { get; }

        }
    }

}


