using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomNumber { get; set; }

        public int RoomStatusId { get; set; }
        public RoomStatus RoomStatus { get; set; }

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        public Boolean IsBooked { get; set; }

        public ICollection<Booking> Bookings { get; set; }

    }
}
