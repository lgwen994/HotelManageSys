using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models.ViewModels
{
    public class BookingRoomStatusViewModel
    {

        public List<Booking> Bookings { get; set; }
        public RoomStatus RoomStatus { get; set; }

    }
}
