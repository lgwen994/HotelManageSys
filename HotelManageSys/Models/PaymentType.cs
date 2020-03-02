using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class PaymentType
    {
        
        [Key]
        public int PaymentTypeId { get; set; }
        public string Type { get; set; }
        ICollection<Booking> Bookings { get; set; }
    }
}
