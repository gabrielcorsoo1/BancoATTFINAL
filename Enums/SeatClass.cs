using System.ComponentModel.DataAnnotations;

namespace AtlasAir.Enums
{
    public enum SeatClass
    {
        [Display(Name = "Econômica")]
        Economy,

        [Display(Name = "Executiva")]
        Business,

        [Display(Name = "Primeira Classe")]
        FirstClass
    }
}