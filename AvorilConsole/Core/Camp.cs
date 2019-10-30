using System.Collections.Generic;
///
using AvorilConsole.Entities;

namespace AvorilConsole.Core
{
    // Core Namespace

    public class Camp
    {
        public Camp(List<Slave> _Slaves, Hero _Hero, Vector2 _Position)
        {
            if (_Slaves == null)
                _Slaves = new List<Slave>();
            Slaves = _Slaves;
            Hero = _Hero;
            Position = _Position;
        }

        // Все рабы принадлежащие игроку
        public List<Slave> Slaves { get; private set; }

        // Герой подконтрольный игроку
        public Hero Hero { get; private set; }
        
        // Координаты блока на карте мира
        private Vector2 Position { get; set; }

        
        public Vector2 GetWorldPosition() => Position;
        public void SetWorldPosition(Vector2 _Position) => Position = _Position;


    }
}
