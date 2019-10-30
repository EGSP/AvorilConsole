using AvorilConsole.Entities;

namespace AvorilConsole.Diseases
{
    // Кашель отнимает 2 хп у героя
    public class Cough : DiseaseEvent
    {
        public Cough()
        {
            Name = "Cough";
        }

        public override DiseaseEvent Invoke(Hero _Hero)
        {
            _Hero.Damage(2);

            return new DiseaseFactory().CreateDefaultDisease();
        }
    }

}
