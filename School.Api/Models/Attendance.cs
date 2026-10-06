namespace School.Api.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int? DivisionId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty; // "Present", "Absent", "Leave"
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
 
        // optional convenience fields for frontend
        public string? StudentName { get; set; }
        public string? ClassName { get; set; }
        public string? DivisionName { get; set; }
    }
}
