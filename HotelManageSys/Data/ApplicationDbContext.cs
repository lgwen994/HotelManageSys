using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelManageSys.Models;

namespace HotelManageSys.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HotelManageSys.Models.Booking> Booking { get; set; }
        public DbSet<HotelManageSys.Models.Customer> Customer { get; set; }
        public DbSet<HotelManageSys.Models.Parking> Parking { get; set; }
        public DbSet<HotelManageSys.Models.PaymentType> PaymentType { get; set; }
        public DbSet<HotelManageSys.Models.Room> Room { get; set; }
        public DbSet<HotelManageSys.Models.RoomStatus> RoomStatus { get; set; }
        public DbSet<HotelManageSys.Models.RoomType> RoomType { get; set; }
    }
}
