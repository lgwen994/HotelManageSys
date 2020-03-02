using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class Booking
    {

        [Key]
        public int BookingId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Boolean IsPaid { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public Boolean NeedParking { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int ParkingId { get; set; }
        public Parking Parking { get; set; }
    }
}
