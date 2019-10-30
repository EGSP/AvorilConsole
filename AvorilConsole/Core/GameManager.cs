using System;
using AvorilConsole.Core;

namespace AvorilConsole
{
    // Core Namespace

    public class GameManager
    {
        public GameManager()
        {
            if(Instance != null)
                throw new SystemException("Вы пытаетесь создать ещё один GameManager, однако существовать может лишь один");

            Instance = this;

            WorldMap = WorldMap.GenerateMap(new Vector2(31, 16));
        }

        public static GameManager Instance;

        // Текущая карта мира
        public WorldMap WorldMap { get; private set; }
    }
    
}
