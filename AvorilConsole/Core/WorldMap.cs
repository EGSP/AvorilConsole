using System;

namespace AvorilConsole.Core
{
    public class WorldMap
    {
        public WorldMap(Vector2 _Size,WorldBlock[,] _Blocks)
        {
            Size = _Size;
            Blocks = _Blocks;
        }

        public Vector2 Size { get; private set; }

        public WorldBlock[,] Blocks;

        
        public WorldBlock GetBlock(Vector2 _Position)
        {
            if (_Position.x > Size.x || _Position.y > Size.y)
                throw new Exception($"GetBlock координаты вне границ мира Size({Size.ToString()}); Postion({_Position.ToString()})");

            return Blocks[_Position.x, _Position.y];
        }

        // Генерация карты по заданным параметрам
        public static WorldMap GenerateMap(Vector2 _Size)
        {
            WorldBlock[,] blocks = new WorldBlock[_Size.x, _Size.y];
            
            for (int x = 0; x < _Size.x; x++)
            {
                for(int y = 0; y < _Size.y; y++)
                {
                    // Пустой блок с пустым событием (нужнен только для дебага)
                    var newBlock = new WorldBlock(WorldBlockType.Neutral, new Vector2(x, y), "null");

                    // Определение соседей по ОсиX
                    if (x > 0)
                    {
                        newBlock.SetNeighbour(blocks[x - 1, y], WorldBlock.WorldBlockSide.Left);
                        blocks[x - 1, y].SetNeighbour(newBlock, WorldBlock.WorldBlockSide.Right);
                    }

                    // Определение соседей по ОсиY
                    if (y > 0)
                    {
                        newBlock.SetNeighbour(blocks[x, y - 1], WorldBlock.WorldBlockSide.Down);
                        blocks[x, y - 1].SetNeighbour(newBlock, WorldBlock.WorldBlockSide.Up);
                    }
                    
                    blocks[x, y] = newBlock;

                }
            }

            return new WorldMap(_Size,blocks);
        }
    }


    public enum WorldBlockType
    {
        Empty,      // Пустой блок на котором ничего не происходит и на него нельзя ступить
        Neutral,    // Нейтральный блок в котором 100% не будет врагов
        Dangerous,  // Опасный блок в котором с некоторой вероятностью могут появиться враги
        Assault     // Блок битвы (100%) вероятность битвы
    }

    // Ячейка мира
    public class WorldBlock
    {
        public WorldBlock(WorldBlockType _Type, Vector2 _Position, string _EventID)
        {
            Type = _Type;
            Position = _Position;
            EvendID = _EventID;

            Neighbors = new WorldBlock[4];
        }

        // Относительная сторона от this
        public enum WorldBlockSide
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }

        public WorldBlockType Type { get; protected set; } // Тип блока
        public Vector2 Position { get; protected set; } // Позиция ячейки

        protected WorldBlock[] Neighbors { get; set; } // Ссылки на соседей
        // 0 - Left
        // 1 - Right
        // 2 - Up
        // 3 - Down

        public WorldBlock this[WorldBlockSide _Side]
        {
            get
            {
                return Neighbors[(int)_Side];
            }
        }

        // Ссылки на соседние клетки
        public WorldBlock Left { get => Neighbors[0]; set => Neighbors[0] = value; }
        public WorldBlock Right { get => Neighbors[1]; set => Neighbors[1] = value; }
        public WorldBlock Up { get => Neighbors[2]; set => Neighbors[2] = value; }
        public WorldBlock Down { get => Neighbors[3]; set => Neighbors[3] = value; }

        protected string EvendID { get; set; }

        // Завершил ли игрок событие на этом месте
        public bool IsCompleted { get; protected set; }

        // Установка соседа
        public void SetNeighbour(WorldBlock _Neighbour, WorldBlockSide _Side)
        {
            Neighbors[(int)_Side] = _Neighbour;
        }


        public override string ToString()
        {
            return Position.ToString();
        }
    }
    

    public class WorldBlockEvent
    {
        public enum WorldBlockEventState
        {
            IntroMonolog = 1,
            IntroDialogs = 2,
            Battle = 3,
            EndMonolog = 4,
            FreeTime = 5

        }
    
    }

}
