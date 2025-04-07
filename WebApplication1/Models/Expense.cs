namespace WebApplication1.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int BookingID { get; set; }
        public required string ExpenseDescription { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public required Booking Booking { get; set; }
    }
}
