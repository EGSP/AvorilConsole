using System;
using AvorilConsole.Diseases;

namespace AvorilConsole.Items
{
    public class BloodItem : StackableItem
    {
        public BloodItem(DiseaseEvent _Disease, int _Count) : base(_Disease, _Count) 
        {
            Type = CollectableItemType.Liquid;
            Name = "Blood";
        }

        public override ICollectableItem GetCopy()
        {
            var copy = new BloodItem(Disease, Count);
            return copy;
        }
        
        public override ICollectableItem GetRange(int _Count)
        {
            if (Count == 0)
                throw new SystemException("BloodItem.Count равно нулю, однако вы пытаетесь вытащить часть (GetRange)!");

            if (_Count <= 0)
                _Count = 1;

            if (_Count > Count)
                _Count = Count;

            var rangedCopy = new BloodItem(Disease, _Count);

            Count -= _Count;

            return rangedCopy;
        }
    }

}
