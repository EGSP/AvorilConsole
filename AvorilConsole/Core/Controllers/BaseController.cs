
using System.Collections.Generic;
///
namespace AvorilConsole.Core.Input.Controllers
{
    /// <summary>
    ///  Базовый класс контроллера, от него должны наследоваться все контроллеры 
    /// </summary>
    public abstract class BaseController : IPlayerController
    {
        public BaseController(Camp _Camp)
        {
            Camp = _Camp;

            Commands = InitializeCommands();
        }

        public PlayerControllerType Type { get; protected set; }

        public Camp Camp { get; }
        
        public Dictionary<string, ControllerActionDelegate> Commands { get; protected set; }

        public abstract void DoCommand(PlayerControllerAction action);

        protected abstract Dictionary<string, ControllerActionDelegate> InitializeCommands();

        /// <summary>
        /// Запрос на изменение текущего контроллера в PlayerInput
        /// </summary>
        /// <param name="newPlayerController"> Новый контроллер другого типа</param>
        protected abstract void CallPIToChangeController(IPlayerController newPlayerController);
    }
}
