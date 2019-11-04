
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
            if (_Camp == null)
                throw new System.Exception("Null _Camp in ControllerConstructor");

            Camp = _Camp;

            Commands = InitializeCommands();
        }

        public PlayerControllerType Type { get; protected set; }

        public Camp Camp { get; }
        
        public Dictionary<string, ControllerActionDelegate> Commands { get; protected set; }

        public virtual void DoCommand(PlayerControllerAction action)
        {
            var command = Commands.GetValueOrDefault(action.FunctionName);

            // Если подобной команды не существует
            if (command == null)
            {
                throw new System.Exception(Type.ToString()+" не имеет метода " + action.FunctionName);
            }
            else
            {
                Log.print(Type.ToString() + ": Invoke " + action.FunctionName);
                command.Invoke(action.Argument);
            }
        }

        protected abstract Dictionary<string, ControllerActionDelegate> InitializeCommands();

        /// <summary>
        /// Запрос на изменение текущего контроллера в PlayerInput
        /// </summary>
        /// <param name="newPlayerController"> Новый контроллер другого типа</param>
        protected abstract void CallPIToChangeController(IPlayerController newPlayerController);
    }
}
