using Microsoft.AspNetCore.Identity;

namespace RentACars.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double PricePerDay { get; set; } // Ако използвате double за цена, няма нужда от конверсия
        public string Location { get; set; }
        public string ImageUrl { get; set; }

        // Добавяме свойството за UserId
        public string UserId { get; set; }

        // Може да добавите и навигационно свойство за връзка с IdentityUser
        public virtual IdentityUser User { get; set; }
    }
}
