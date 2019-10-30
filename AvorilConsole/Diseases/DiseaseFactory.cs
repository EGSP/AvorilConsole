namespace AvorilConsole.Diseases
{
    // Все болезни должны создаваться через эту фабрику
    public class DiseaseFactory
    {
        // Создаёт и возвращает болезнь-пустышку
        public DiseaseEvent CreateDefaultDisease()
        {
            return new DefaultDisease();
        }
    }

}
