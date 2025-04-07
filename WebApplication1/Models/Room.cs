namespace WebApplication1.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public required string RoomNumber { get; set; }
        public required string RoomType { get; set; }
        public bool IsAvailable { get; set; }
        public required string ImagePath { get; set; } // Added missing properties
        public required string RoomName { get; set; }
        public double Rating { get; set; }
        public decimal Price { get; set; }
        public required string DetailsLink { get; set; }
    }
}
