using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        public string CardNum { get; set; }

        public string From { get; set; }

        public string Company { get; set; }

        public string TravelAgency { get; set; }

        public ICollection<Booking> Bookings { get; set; }

    }
}
