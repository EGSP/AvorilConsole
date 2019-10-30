using AvorilConsole.Diseases;

namespace AvorilConsole.Items
{
    public enum CollectableItemType
    {
        NaN,
        Liquid
    }

    public interface ICollectableItem
    {
        CollectableItemType Type { get; }

        string Name { get; }
        
        bool Stackable { get; }
        
        int Count { get; }
        
        // Активировать болезнь должна сущность, которая подбирает этот предмет
        DiseaseEvent Disease { get; }
        
        // Добавление объекта если возможно
        void AddRange(int _Count);

        // Получение нескольких экземпляров при Count > 0
        // Всегда возвращает 1 или более экземпляров
        ICollectableItem GetRange(int _Count);

        // Получение копии всего объекта
        ICollectableItem GetCopy();
    }

    public abstract class StackableItem : ICollectableItem
    {
        public StackableItem(DiseaseEvent _Disease, int _Count)
        {
            Disease = _Disease;
            Count = _Count;

            Stackable = true;
        }

        public CollectableItemType Type { get; protected set; }

        public string Name { get; protected set; }

        public DiseaseEvent Disease { get; protected set; }

        public bool Stackable { get; }

        // Количество подобных объектов в ячейке
        public int Count
        {
            get => count;
            protected set
            {
                count = value;

                if (value < 0)
                    count = 0;
            }
        }

        protected int count;

        // Добавление некоторого числа экземпляров 
        public void AddRange(int _Count)
        {
            if (_Count < 0)
                return;

            Count += _Count;
        }

        public abstract ICollectableItem GetCopy();

        public abstract ICollectableItem GetRange(int _Count);
    }
}
