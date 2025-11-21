using System.ComponentModel.DataAnnotations;

namespace AtlasAir.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "O telefone deve conter apenas números (8 a 15 dígitos).")]
        [Display(Name = "Telefone")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // apenas para testes locais
        public bool IsAdmin { get; set; } = false;
    }
}