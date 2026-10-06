namespace SchoolApi.Models
{
    public class Division
    {
        public int DivisionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string? ClassName { get; set; }   // <- required if you want to show it
        public DateTime CreatedAt { get; set; }
    }



}


