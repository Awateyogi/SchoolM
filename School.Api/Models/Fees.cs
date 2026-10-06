namespace SchoolApi.Models
{
    public class Fee
    {
        public int FeeId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }   // linked to student’s class
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
       // public decimal Balance { get; set; }
       // public DateTime DueDate { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
