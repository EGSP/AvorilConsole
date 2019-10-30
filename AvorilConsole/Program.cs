using System;
using AvorilConsole.Core.Input;
using AvorilConsole.Core.Input.Controllers;
using AvorilConsole.Entities;
using AvorilConsole.Entities.EntityInterfaces;
using AvorilConsole.Entities.Factory;
using AvorilConsole.Core;
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

            var gameManager = new GameManager();

            var travelController = new TravelController(new Camp(null,
                new Hero(new EntityFactory.BaseEntityPattern(
                    EntityFactory.CreateLifeEntity(LifeState.Alive, _MaxHealth: 10, _Health: 9),
                    EntityFactory.CreateAttackEnitity(_HitDamage: 1, _NextAttack: new AttackStructure(), _BaseEffects: null, _IsStunned: false))),
                new Vector2(0, 0)));

            var playerInput = new PlayerInput(travelController);

            for (int x = 0; x < 6; x++)
                playerInput.AddPlayerControllerAction(new PlayerControllerAction(PlayerControllerType.TravelController, "Move", WorldBlock.WorldBlockSide.Right));

            for (int y = 0; y < 3; y++)
                playerInput.AddPlayerControllerAction(new PlayerControllerAction(PlayerControllerType.TravelController, "Move", WorldBlock.WorldBlockSide.Up));

            playerInput.UpdateAll();

            Log.print(travelController.Camp.GetWorldPosition());

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
        public BattleController(Camp _Camp, BaseEnemy _Enemy):base(_Camp)
        {
            Enemy = _Enemy;
        }

        
        private BaseEnemy Enemy;
        

        public override void DoCommand(PlayerControllerAction action)
        {
            throw new NotImplementedException();
        }

        protected override void CallPIToChangeController(IPlayerController newPlayerController)
        {
            PlayerInput.Instance.ChangePlayerController(new TravelController(Camp));
        }

        protected override Dictionary<string, ControllerActionDelegate> InitializeCommands()
        {
            throw new NotImplementedException();
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

            attackIndex++;
            if (attackIndex >= Attacks.Count)
                attackIndex = 0;

            NextAttack = Attacks[attackIndex];

            return couple;
        }
    }
}
