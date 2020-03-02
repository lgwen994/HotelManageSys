using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class RoomStatus
    {
        [Key]
        public int RoomStatusId { get; set; }

        public string Status { get; set; }
        ICollection<Room> Rooms { get; set; }
        
    }
}
