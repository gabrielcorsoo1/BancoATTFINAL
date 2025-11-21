using System.ComponentModel.DataAnnotations;

namespace AtlasAir.ViewModels
{
    public class LoginViewModel
    {
        // Aceita email OU telefone
        [Required]
        public string UserIdentifier { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}