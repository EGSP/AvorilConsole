using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using AvorilConsole.Entities.EntityInterfaces;

namespace AvorilConsole.Entities.Factory
{
    public static class EntityFactory
    {
        private static Dictionary<string, BaseEnemy> EnemyPatterns;

        /// <summary>
        /// Метод инициализации фабрики сущностей
        /// </summary>
        /// <param name="_BaseEnemiesPath">Путь к каталогу в котором находится информация о врагах</param>
        public static void Initialize(string _BaseEnemiesPath)
        {
            var FoldersPath = Directory.GetDirectories(_BaseEnemiesPath);
            string[] Folders = new string[FoldersPath.Length];
            // Получение имён директорий
            for (int f = 0; f < FoldersPath.Length; f++)
            {
                Folders[f] = new DirectoryInfo(FoldersPath[f]).Name;
            }

            for(int i = 0; i < Folders.Length; i++)
            {
                // Если есть файл с настройками врага
                if (File.Exists(_BaseEnemiesPath + Folders[i] + "\\" + Folders[i] + ".monst"))
                {
                    FileStream fs = new FileStream(_BaseEnemiesPath + Folders[i] + "\\" + Folders[i] + ".monst", FileMode.Open,FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);

                    string data = sr.ReadToEnd();

                    sr.Close();
                    fs.Close();

                    CreateInstanceOfEnemy(data);
                    
                }
                else
                {
                    throw new System.Exception(_BaseEnemiesPath + Folders[i] + " does not contain .monst file!");
                }
            }

        }

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

        /// <summary>
        /// Загружает все копии врагов с настройками
        /// </summary>
        /// <param name="data"> Строка с настрйоками врага</param>
        /// <returns></returns>
        public static void CreateInstanceOfEnemy(string _data)
        {
            // Заменить принетры на специальные для фабрики
            // Type
            var reg = new Regex("Type:(\\w+)");
            var typestring = reg.Match(_data).Groups[0].Value;

            // MaxHealth
            reg = new Regex("MaxHealth:(\\w+)");
            var maxhealth = int.Parse(reg.Match(_data).Groups[0].Value);

            reg = new Regex("Health:(\\w+)");
            var health =    int.Parse(reg.Match(_data).Groups[0].Value);

            reg = new Regex("HitDamage:(\\w+)");
            var hitdamage = int.Parse(reg.Match(_data).Groups[0].Value);

            // Выделяем строку с атаками
            reg = new Regex("(Attacks:\\S+;)");
            string astring = reg.Match(_data).Groups[0].Value;

            // Выделяем атаки
            reg = new Regex("(\\d+)");
            var attacks = reg.Matches(astring);

            AttackStructure[] attackStructures = new AttackStructure[attacks.Count];
            for(int i = 0; i < attacks.Count; i++)
            {
                attackStructures[i] = AttackStructure.ToAttackStructure(attacks[i].Value);
            }

            //var IA = CreateAttackEnitity(_HitDamage: hitdamage,_NextAttack: new AttackStructure,)

        }

        // ------------------------------------------------------------- P A T T E R N S -----------------------------------------------------

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

            public void ComputeDamage(AttackInfo _EnemyAttackInfo)
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


