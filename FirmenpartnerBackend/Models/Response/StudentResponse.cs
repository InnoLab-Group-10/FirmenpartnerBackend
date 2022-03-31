﻿namespace FirmenpartnerBackend.Models.Response
{
    public class StudentBaseResponse : PersonBaseResponse
    {
        public Guid? Id { get; set; }
        public string StudentId { get; set; }
        public int Semester { get; set; }
        public Data.Program Program { get; set; }
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
}