﻿namespace FirmenpartnerBackend.Models.Response
{
    public class MailingListEntryResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string? Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Company { get; set; }
        public string Email { get; set; }
    }
}
