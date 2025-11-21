using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AtlasAir.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // novo: email para login opcional
        public string Email { get; set; } = string.Empty;

        // novo: hash da senha (não armazenamos senha em texto plano)
        public string PasswordHash { get; set; } = string.Empty;

        // novo: flag para identificar administrador
        public bool IsAdmin { get; set; } = false;

        // mantido: marcador existente (pode continuar sendo usado)
        public bool IsSpecialCustomer { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
