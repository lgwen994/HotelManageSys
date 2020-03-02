using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManageSys.Models.ViewModels
{
    public class BookingRoomTypeViewModel
    {

        //public string MovieGenre { get; set; }
        //public string SearchString { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime SearchDate { get; set; }
        public Booking Booking;
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        //public SelectList Genres;
    }
}
