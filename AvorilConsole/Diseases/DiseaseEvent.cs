using AvorilConsole.Entities;

namespace AvorilConsole.Diseases
{
    // Событие болезни
    public abstract class DiseaseEvent
    {
        public string Name { get; protected set; }

        // Активирует болезнь и возвращает следующий этап болезни (обычно DefaultDisease)
        public abstract DiseaseEvent Invoke(Hero _Hero);
    }

}
