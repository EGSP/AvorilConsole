using System;
using System.Collections.Generic;
///
using AvorilConsole.Core.Input.Controllers;

namespace AvorilConsole.Core.Input
{
    // Core.Input Namespace -----------------------------

    public class PlayerInput
    {
        public PlayerInput(IPlayerController _PlayerController)
        {
            if (_PlayerController == null)
                throw new SystemException("Null аргумент _PlayerController в PlayerInput => _PlayerController всегда должен быть определён");

            if (Instance != null)
                throw new SystemException("Вы пытаетесь создать ещё один PlayerInput, однако существовать может лишь один");

            Instance = this;

            PlayerController = _PlayerController;
            PlayerControllerActions = new Queue<PlayerControllerAction>();
        }

        public static PlayerInput Instance { get; private set; }

        private IPlayerController PlayerController; // Текущий обработчик управления игрока
        private Queue<PlayerControllerAction> PlayerControllerActions; // Очередь действий для контроллера

        public void Update()
        {
            if (PlayerControllerActions.Count > 0)
            {
                var action = PlayerControllerActions.Dequeue();

                // Если действие относится к текущему контроллеру, то оно обрабатывается
                if (action.ControllerType == PlayerController.Type)
                {
                    Console.WriteLine("DoCommand " + action.FunctionName);
                    PlayerController.DoCommand(action);
                }
            }
        }

        public void UpdateAll()
        {
            var count = PlayerControllerActions.Count;
            for(int i = 0;i<count;i++)
            {
                var action = PlayerControllerActions.Dequeue();

                // Если действие относится к текущему контроллеру, то оно обрабатывается
                if (action.ControllerType == PlayerController.Type)
                {
                    //Console.WriteLine($"DoCommand{i} " + action.FunctionName );
                    PlayerController.DoCommand(action);
                }
            }
        }

        /// <summary>
        ///  Добавляет новое действие в очередь задач
        /// </summary>
        public void AddPlayerControllerAction(PlayerControllerAction action)
        {
            PlayerControllerActions.Enqueue(action);
        }

        /// <summary>
        /// Данный метод вызывается самими контроллерами, т.к. только контроллеры знают об их изменчивости (Все действия будут удалены)
        /// </summary>
        /// <param name="_PlayerController"> Новый контролеер</param>
        public void ChangePlayerController(IPlayerController _PlayerController)
        {
            PlayerControllerActions.Clear();

            PlayerController = _PlayerController;
        }
    }
}
