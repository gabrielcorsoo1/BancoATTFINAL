using System.ComponentModel.DataAnnotations;

namespace AtlasAir.Enums
{
    public enum ReservationStatus
    {
        [Display(Name = "Pendente")]
        Pending,

        [Display(Name = "Confirmada")]
        Confirmed,

        [Display(Name = "Check-in Realizado")]
        CheckedIn,

        [Display(Name = "Embarcada")]
        Boarded,

        [Display(Name = "Cancelada")]
        Cancelled
    }
}
