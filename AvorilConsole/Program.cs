using System;
using AvorilConsole.Core.Input;
using AvorilConsole.Core.Input.Controllers;
using AvorilConsole.Entities;
using AvorilConsole.Entities.EntityInterfaces;
using AvorilConsole.Entities.Factory;
using AvorilConsole.Core;
using AvorilConsole.Items;
using AvorilConsole.Items.Factory;
using System.Collections.Generic;

public struct Vector2
{

    public Vector2(int _x,int _y)
    {
        x = _x;
        y = _y;
    }

    public int x; 
    public int y;

    public override string ToString()
    {
        return "X:" + x.ToString() + " Y:" + y.ToString();
    }
}

namespace AvorilConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Устанавливаем стандартный способ вывода информации ЛОГ
            Log.SetDefaultPrinter(new DesktopConsolePrinter());

            FileSystemManager.CheckAllFoldersExistance();
            EntityFactory.Initialize(FileSystemManager.BaseEnemiesPath);

            //var gameManager = new GameManager();

            //var travelController = new TravelController(new Camp(null,
            //    new Hero(new EntityFactory.BaseEntityPattern(
            //        EntityFactory.CreateLifeEntity(LifeState.Alive, _MaxHealth: 10, _Health: 9),
            //        EntityFactory.CreateAttackEnitity(_HitDamage: 1, _NextAttack: new AttackStructure(), _BaseEffects: null, _IsStunned: false))),
            //    new Vector2(0, 0)));

            //var playerInput = new PlayerInput(travelController);

            //for (int x = 0; x < 6; x++)
            //    playerInput.AddPlayerControllerAction(new PlayerControllerAction(PlayerControllerType.TravelController, "Move", WorldBlock.WorldBlockSide.Right));

            //for (int y = 0; y < 3; y++)
            //    playerInput.AddPlayerControllerAction(new PlayerControllerAction(PlayerControllerType.TravelController, "Move", WorldBlock.WorldBlockSide.Up));

            //playerInput.UpdateAll();

            //Log.print(travelController.Camp.GetWorldPosition());

        }
    }


    public class Couple<T, U>
    {
        public Couple(T _a,U _b)
        {
            a = _a;
            b = _b;
        }

        public T a { get; }
        public U b { get; }
    }
    

    public struct AttackStructure
    {
        public AttackStructure(int _AoD, int _Stand, int _Core)
        {
            AoD = _AoD;
            Stand = _Stand;
            Core = _Core;
        }

        // Attack(1) or Defence(0)
        public int AoD { get; }
        // Стойка (0 - низ, 1 - середина, 2 - верх)
        public int Stand { get; }
        // Ядро (тёмное(0) и кровавое(1))
        public int Core { get; }

        public override string ToString()
        {
            return AoD.ToString() + Stand.ToString() + Core.ToString();
        }

        public static AttackStructure ToAttackStructure(string _AttackStructure)
        {
            int _AoD = int.Parse(_AttackStructure[0].ToString());
            int _Stand = int.Parse(_AttackStructure[1].ToString());
            int _Core = int.Parse(_AttackStructure[2].ToString());

            var attackStructure = new AttackStructure(_AoD,_Stand,_Core);
            return attackStructure;
        }
    }

    public class AttackInfo
    {
        public AttackInfo(int _DamageCount)
        {
            DamageCount = _DamageCount;
        }

        // Количество урона
        public int DamageCount;
    }

    public interface IAttackEntity
    {
        // Сила удара
        int HitDamage { get; }
        // Следующая атака
        AttackStructure NextAttack { get; }
        // Базовые эффекты, которые существуют всегда
        List<CastableEffect> BaseEffects { get; }
        // Наложенные эффекты
        List<EmmitableEffect> EmmitableEffects { get; }
        // Застанена ли сущность
        bool IsStunned { get; }

        Couple<AttackStructure,AttackInfo> DoAttack();
        List<EmmitableEffect> GetEffects(string cas, Couple<AttackInfo, ILifeEntity> _Myself, AttackInfo _Oneself);
        void AddEffects(List<EmmitableEffect> _Effects);
        void ComputeDamage(AttackInfo _EnemyAttackInfo);

    }

    // Произносимое заклинание, которое возвращает эффект
    public abstract class CastableEffect
    {
        public CastableEffect(int _OwnerID)
        {
            OwnerID = _OwnerID;
        }

        // Ключ активации 
        public abstract string Key { get; }

        // Entity`s InstanceID casted this effect
        public int OwnerID { get; private set; }

        // Возвращает накладываемый эффект заклинания
        public abstract EmmitableEffect Cast(Couple<AttackInfo, ILifeEntity> _Owner, AttackInfo _Target);
    }

    // Накладываемый эффект
    public abstract class EmmitableEffect
    {
        // Количество оставшихся ходов до исчезновения
        public int Steps { get; protected set; }

        // Взаимодействует с Живой сущностью
        public abstract void Emmit(ILifeEntity _Entity);
    }


    public class BattleController : BaseController
    {
        public BattleController(Camp _Camp, LinkedList<BaseEnemy> _Enemies):base(_Camp)
        {
            Type = PlayerControllerType.BattleController;
            
            if (_Enemies == null || _Enemies.Count == 0)
                throw new System.Exception("Activated Battlecontroller without BaseEnemies");

            Enemies = _Enemies;
            NextEnemy = Enemies.First;
        }

        // Список врагов в событии
        private LinkedList<BaseEnemy> Enemies;
        // Следующий враг
        private LinkedListNode<BaseEnemy> NextEnemy;


        
        protected override void CallPIToChangeController(IPlayerController newPlayerController)
        {
            PlayerInput.Instance.ChangePlayerController(new TravelController(Camp));
        }

        protected override Dictionary<string, ControllerActionDelegate> InitializeCommands()
        {
            var _Commands = new Dictionary<string, ControllerActionDelegate>();
            _Commands.Add("DoBattle", DoBattle);

            return _Commands;
        }
        
        // Compute attack action between hero and enemy (null argument)
        private void DoBattle(object argument)
        {
            var hero = Camp.Hero;

            var enemy = NextEnemy.Value;
            // Меняем противника для следующего хода
            NextEnemy = NextEnemy.Next;
            // Проверка на конец списка
            if (NextEnemy == null)
                NextEnemy = Enemies.First;

            // Couple<AttackStructure(struct), AttackInfo(class)>
            var heroAttack = hero.DoAttack();
            var enemyAttack = enemy.DoAttack();

            var casforhero = ComputeAttackString(heroAttack.a, enemyAttack.a);
            var casforenemy = ComputeAttackString(enemyAttack.a, heroAttack.a);

            var emmitableEffectsForEnemy = hero.GetEffects(casforhero, new Couple<AttackInfo, ILifeEntity>(heroAttack.b, hero), enemyAttack.b);
            var emmitableEffectsForHero = enemy.GetEffects(casforenemy, new Couple<AttackInfo, ILifeEntity>(enemyAttack.b, enemy), heroAttack.b);

            hero.AddEffects(emmitableEffectsForHero);
            enemy.AddEffects(emmitableEffectsForEnemy);

            hero.ComputeDamage(enemyAttack.b);
            enemy.ComputeDamage(heroAttack.b);

            // Завершение боевого хода
            hero.BattleTurn();
            enemy.BattleTurn();

            // Смерть героя
            if(hero.Health <= 0)
            {
                Log.print("Hero health is zero");
            }

            // Смерть врага
            if(enemy.Health <= 0)
            {
                Log.print(enemy.GetInstanceID() + ": is Dead");
                Enemies.Remove(enemy);

                // Если все противники уничтожены
                if(Enemies.Count == 0)
                {
                    CallPIToChangeController(new TravelController(Camp));
                }
            }

        }

        private string ComputeAttackString(AttackStructure _Myself,AttackStructure _Oneself)
        {
            var cas = _Myself.ToString() + _Oneself.ToString();

            return cas;
        }
    }


    public abstract class BaseEnemy : BaseEntity
    {
        public BaseEnemy(EntityFactory.BaseEntityPattern _BaseEntityPattern, List<AttackStructure> _Attacks) : base(_BaseEntityPattern)
        {
            if(_Attacks == null || _Attacks.Count == 0)
            {
                _Attacks.Add(new AttackStructure());
            }

            Attacks = _Attacks;
        }

        private List<AttackStructure> Attacks;
        private int attackIndex;

        // Добавлена смена следующей атаки
        public override Couple<AttackStructure, AttackInfo> DoAttack()
        {
            var couple = base.DoAttack();

            NextAttack = Attacks[attackIndex];
            attackIndex++;
            if (attackIndex >= Attacks.Count)
                attackIndex = 0;
            
            return couple;
        }
    }

    //
    //--------------------------------------------------------- I N V E N T O R Y ----------------------------------------------
    //

    public class Inventory
    {
        public Inventory(List<Couple<int,int>> _IDandCount)
        {
            // Число 12 взято из записей о разработке (12 предметов на 36 видов атак)
            Items = new Item[12];

            if (_IDandCount.Count > 12 || _IDandCount.Count == 0)
                throw new System.Exception("_ItemsID count in inventory constructor is larger 12 or equal 0");

            // Добавление предметов в массив
            for (int i = 0; i < _IDandCount.Count; i++)
            {
                var couple = _IDandCount[i];
                var newItem = ItemFactory.CreateItem(couple.a, couple.b);

                Items[i] = newItem;
            }

        }

        /// <summary>
        /// Нельзя изменять элементы массива из вне
        /// </summary>
        public Item[] Items;

        // Возвращает null, если Item был вставлен, иначе возвращается сам предмет, однако Count может изменится
        public Item InsertItem(Item _NewItem)
        {
            for(int i = 0; i < Items.Length; i++)
            {
                var inventoryItem = Items[i];

                // Если ячейка пуста вставляем предмет
                if(inventoryItem == null)
                {
                    Items[i] = _NewItem;
                    return null;
                }

                // Если совпал ID
                if(inventoryItem.ID == _NewItem.ID)
                {
                    // Попытка добавить хотя бы часть предмета
                    _NewItem.Count = inventoryItem.Insert(_NewItem.Count);

                    // Предмет был успешно добавлен
                    if(_NewItem.Count == 0)
                    {
                        return null;
                    }
                }
            }

            // Если предмет был хотя бы частично добавлен
            return _NewItem;
        }

        public Item RemoveItem(int _Index)
        {
            if (_Index < 0 || _Index > 11)
                throw new System.ArgumentOutOfRangeException("RemoveItem index is below 0 or larger 11");

            var item = Items[_Index];
            Items[_Index] = null;

            return item;
        }

    }

    // Сущность, которая имеет инвентарь
    public interface IInventoryEntity
    {
        Inventory Inventory { get; }
    }
}
