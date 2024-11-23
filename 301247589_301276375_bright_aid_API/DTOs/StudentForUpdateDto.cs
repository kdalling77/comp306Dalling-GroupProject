namespace _301247589_301276375_bright_aid_API.DTOs
{
    public class StudentForUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryOfOrigin { get; set; }
        public string? Gender { get; set; }


        public string? InstitutionName { get; set; }
        public string? ProgramOfStudy { get; set; }
        public int? YearOfStudy { get; set; }
        public double? GPA { get; set; }


        public decimal? RequestedAmount { get; set; }
        public string? FinancialNeedDescription { get; set; }

        public bool? HasDonor { get; set; }
        public long? DonorId { get; set; }
        public string? RegistrationDate { get; set; }
    }
}
