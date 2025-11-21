using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AtlasAir.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "O telefone deve conter apenas números (8 a 15 dígitos).")]
        public string Phone { get; set; } = string.Empty;

        // email para login opcional
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        // hash da senha (não armazenamos senha em texto plano)
        public string PasswordHash { get; set; } = string.Empty;

        // novo: flag para identificar administrador
        public bool IsAdmin { get; set; } = false;

        // mantido: marcador existente (pode continuar sendo usado)
        public bool IsSpecialCustomer { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
