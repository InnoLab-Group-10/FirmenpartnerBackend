﻿namespace FirmenpartnerBackend.Models.Response
{
    public class ChangePasswordResponse : IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
