using System.ComponentModel.DataAnnotations;

namespace AtlasAir.Enums
{
    public enum FlightStatus
    {
        [Display(Name = "Programado")]
        Scheduled,
        
        [Display(Name = "Embarcando")]
        Boarding,
        
        [Display(Name = "Em Voo")]
        InFlight,
        
        [Display(Name = "Concluído")]
        Completed,

        [Display(Name = "Cancelado")]
        Cancelled,

        [Display(Name = "Atrasado")]
        Delayed
    }
}
