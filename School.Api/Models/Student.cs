namespace SchoolApi.Models
{
    public class Student
    {
        
       
            public int StudentId { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public DateTime? DateOfBirth { get; set; }
            public string? Gender { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public int? ClassId { get; set; }
            public string? ClassName { get; set; }
             public int? DivisionId { get; set; }
             public string DivisionName { get; set; }
            public DateTime CreatedAt { get; set; }
       


    }

}
