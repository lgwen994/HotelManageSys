using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models
{
    public class Parking
    {
        public static int NO_PARKING = -1;
        [Key]
        public int ParkingId { get; set; }
        public string ParkingNum { get; set; }

        public Boolean IsAvailable { get; set; }

        [DataType(DataType.Date)]
        public DateTime Testday { get; set; }
    }
}
