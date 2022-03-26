namespace FirmenpartnerBackend.Models.Request
{
    public class StudentRequest : PersonRequest
    {
        public string StudentId { get; set; }

        public int Semester { get; set; }

        public Guid ProgramId { get; set; }
    }
}
