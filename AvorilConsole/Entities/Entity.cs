namespace AvorilConsole.Entities
{
    public abstract class Entity // Является игровой сущностью
    {
        protected int InstanceID;

        public int GetInstanceID() => InstanceID;
    }
}
