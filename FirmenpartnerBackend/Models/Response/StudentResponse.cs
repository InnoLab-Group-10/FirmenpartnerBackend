namespace FirmenpartnerBackend.Models.Response
{
    public class StudentBaseResponse : PersonBaseResponse
    {
        public string StudentId { get; set; }
        public int Semester { get; set; }
        public ProgramBaseResponse Program { get; set; }
    }

    public class StudentCsvBaseResponse : PersonBaseResponse
    {
        public string StudentId { get; set; }
        public int Semester { get; set; }
        public Guid ProgramId { get; set; }
    }


    public class StudentSingleResponse : StudentBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class StudentMultiResponse : IMultiResponse<StudentBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<StudentBaseResponse> Results { get; set; }
    }

    public class StudentCsvMultiResponse : IMultiResponse<StudentCsvBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<StudentCsvBaseResponse> Results { get; set; }
    }
}
