using System;
using Microsoft.AspNetCore.Identity;
using RentACars.Models;

namespace RentACars.Models
{
    public class CarRental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; }
        public DateTime RentDate { get; set; }

        public Car Car { get; set; }
        public IdentityUser User { get; set; }
    }
}
