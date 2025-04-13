namespace RentACars.Models.Cars
{
    public class CreateCarViewModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double PricePerDay { get; set; } // Ако използвате double за цена, няма нужда от конверсия
        public string Location { get; set; }
        public string ImageUrl { get; set; }
    }
}
