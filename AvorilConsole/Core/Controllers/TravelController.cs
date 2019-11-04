using System;
using System.Collections.Generic;
using System.Linq;

using AvorilConsole;
using AvorilConsole.Core;

namespace AvorilConsole.Core.Input.Controllers
{
    /// <summary>
    /// Контроллер, отвечающий за перемещение по карте, взаимодействие с окружением карты
    /// </summary>
    public class TravelController : BaseController
    {
        public enum MoveDir
        {
            Left,
            Right,
            Up,
            Down
        }

        public TravelController(Camp _Camp):base(_Camp)
        {
            Type = PlayerControllerType.TravelController;

            // Получение текущего блока в мире 
            CurrentBlock = GameManager.Instance.WorldMap.GetBlock(_Camp.GetWorldPosition());
        }

        private WorldBlock CurrentBlock;

        
        protected override void CallPIToChangeController(IPlayerController newPlayerController)
        {
            PlayerInput.Instance.ChangePlayerController(newPlayerController);
        }

        // Внесение методов в список команд
        protected override Dictionary<string, ControllerActionDelegate> InitializeCommands()
        {
            var _Commands = new Dictionary<string, ControllerActionDelegate>();
            _Commands.Add("Move", Move);

            return _Commands;
        }
 
        // Перемещаем лагерь
        private void Move(object Argument)
        {
            var nextBlock = CurrentBlock[(WorldBlock.WorldBlockSide)Argument];

            if (nextBlock == null)
            {
                Console.WriteLine("Блока со стороны " + ((WorldBlock.WorldBlockSide)Argument).ToString() + " не существует");
                return;
            }

            CurrentBlock = nextBlock;
            Camp.SetWorldPosition(nextBlock.Position);
            Camp.Hero.WorldTurn();
        }
    }
}
