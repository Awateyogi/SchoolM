using System;
namespace SchoolApi.Models
{
    public class Marks
    {
        public int MarkId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; } = string.Empty;


        public int? Test1 { get; set; }
        public int? Test2 { get; set; }
        public int? Test3 { get; set; }
        public int? Test4 { get; set; }
  
        public int? Sem1 { get; set; }
        public int? Sem2 { get; set; }

       public DateTime CreatedAt { get; set; }

        // Navigation helper
       

    }

}
/// <summary>
/// Summary description for Class1
/// </summary>
