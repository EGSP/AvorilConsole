using AvorilConsole.Diseases;

namespace AvorilConsole.Items
{

    public abstract class Item
    {
        public Item(int _Count)
        {
            Count = _Count;
        }
        
        public abstract int ID { get; }
        public abstract string Name { get; }

        // Текущее количество
        public int Count {
            get
            {
                return count;
            }
            set
            {
                count = value;

                if (count > MaxCount)
                    throw new System.Exception(Name + " count is larger then MaxCount");

                if (count < 0)
                    throw new System.Exception(Name + " count is below zero");
            }
        }
        private int count;

        // Максимальное количество
        public abstract int MaxCount { get; }

        // Получение используемого эффекта
        public abstract CastableEffect Use();

        // Добавление новых экземляров. Невместившиеся экземпляры будут возращены обратно
        public int Insert(int _InsertCount)
        {
            var ost = MaxCount - Count;

            // Если нет места
            if (ost <= 0)
                return _InsertCount;

            // Максимально возможное чилсо новых экземпляров
            var toinsert = System.Math.Clamp(ost, 1, _InsertCount);
            
            Count += toinsert;
            _InsertCount -= toinsert;

            if (_InsertCount < 0)
                throw new System.Exception("_InsertCount in Item.Insert is below zero");

            return _InsertCount;
        }
         
        public Item TakeOne()
        {
            if (Count == 0)
                throw new System.Exception(Name + " count is zero");

            Count--;

            return Clone(1);
        }

        // Возвращает клонированый экземпляр с новым Count
        public abstract Item Clone(int _Count);
    }
}
