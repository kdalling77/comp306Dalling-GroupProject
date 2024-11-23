namespace _301247589_301276375_bright_aid_API.DTOs
{
    public class DonorDto
    {
        // Donor identification
        public int DonorId { get; set; }

        // Donor details
        public string? FullName { get; set; } // Combines FirstName and LastName for easier display
        public string? OrganizationName { get; set; } // For organization-backed donations

        // Financial details
        public decimal? DonationAmount { get; set; } // Amount donated in CAD
        public DateTime? DonationDate { get; set; } // Date of the donation

        // Message to recipient
        public string? MessageToRecipient { get; set; }

        // Preferences
        public string? PreferredRegion { get; set; } // Preferred province or city for donations
        public bool? IsRecurringDonor { get; set; } // Recurring donation indicator
    }
}
