using AvorilConsole.Entities;

namespace AvorilConsole.Diseases
{
    // Пустышка для болезни
    public class DefaultDisease : DiseaseEvent
    {
        public DefaultDisease()
        {
            Name = "DefaultDisease";
        }

        public override DiseaseEvent Invoke(Hero _Hero)
        {
            return new DiseaseFactory().CreateDefaultDisease();
        }
    }

}
