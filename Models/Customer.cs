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

       
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

     
        public string PasswordHash { get; set; } = string.Empty;

    
        public bool IsAdmin { get; set; } = false;

        
        public bool IsSpecialCustomer { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
