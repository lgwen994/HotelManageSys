using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models.ViewModels
{
    public class RoomsCustomerViewModel
    {

        public List<Room> Rooms;
        public Customer Customer { get; set; }
        [DataType(DataType.Date)]
       public DateTime SearchDateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime SearchDateTo { get; set; }

        public int RoomTypeId { get; set; }
    }
}
