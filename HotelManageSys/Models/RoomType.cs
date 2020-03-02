using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

        ICollection<Room> Rooms { get; set; }
    }
}
