namespace _301247589_301276375_bright_aid_API.DTOs
{
    public class StudentDto
    {
        // Basic student details
        public long Id { get; set; }
        public string? FullName { get; set; } // Combines FirstName and LastName for convenience
        public string? Email { get; set; }
        public string? CountryOfOrigin { get; set; }
        public string? Gender { get; set; }

        // Academic details
        public string? InstitutionName { get; set; }
        public string? ProgramOfStudy { get; set; }
        public int? YearOfStudy { get; set; }

        // Financial information
        public decimal? RequestedAmount { get; set; }
        public string? FinancialNeedDescription { get; set; }

        // Donor matching status
        public bool? HasDonor { get; set; }

        // System information
        public string? RegistrationDate { get; set; }

        public string? Password { get; set; }

    }
}
