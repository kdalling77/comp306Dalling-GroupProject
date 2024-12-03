namespace _301247589_301276375_bright_aid_API.Models
{
    public class Student
    {
        // Unique identifier for the student
        public long Id { get; set; }

        // Personal information
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryOfOrigin { get; set; }
        public string? Gender { get; set; }

        // Academic background
        public string? InstitutionName { get; set; }
        public string? ProgramOfStudy { get; set; }
        public int? YearOfStudy { get; set; }
        public double? GPA { get; set; } // Grade Point Average

        // Financial assistance information
        public decimal? RequestedAmount { get; set; }
        public string? FinancialNeedDescription { get; set; } // Explanation of financial hardship

        // Donor connection
        public bool? HasDonor { get; set; } // Indicates if the student is matched with a donor
        public long? DonorId { get; set; } // Reference to the donor (if matched)

        // System tracking
        public string? RegistrationDate { get; set; }

        [System.ComponentModel.DefaultValue(true)] // Default value in the database
        public bool IsActive { get; set; } = true; // Default to true (active)

    }
}
