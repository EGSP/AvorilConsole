using System.Collections.Generic;

namespace AvorilConsole.Core.Input.Controllers
{
    // Контроллер обрабатывающий команды игрока
    public interface IPlayerController
    {
        PlayerControllerType Type { get; }

        Camp Camp { get; }
        
        // Список команд(методов), которые вызывает метод DoCommand
        Dictionary<string, ControllerActionDelegate> Commands { get; }
        
        // Выполнение внешней команды 
        /// <param name="arg"> Name of function </param>
        void DoCommand(PlayerControllerAction action);
    }

    // Типы контроллеров
    public enum PlayerControllerType
    {
        TravelController
    }

    public delegate void ControllerActionDelegate(object Argument);

    // Действие для обработки контроллером 
    public struct PlayerControllerAction
    {
        public PlayerControllerAction(PlayerControllerType _ControllerType, string _FunctionName, object _Argument)
        {
            ControllerType = _ControllerType;
            FunctionName = _FunctionName;
            Argument = _Argument;
        }

        /// <summary>
        /// Тип контроллера, которому отправляется данное действие
        /// </summary>
        public PlayerControllerType ControllerType { get; private set; }
        public string FunctionName { get; private set; }
        public object Argument { get; private set; }
    }

   
}
