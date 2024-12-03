using System.ComponentModel.DataAnnotations;

namespace _301247589_301276375_bright_aid_API.Models
{
    public class Donor
    {
    
        public int DonorId { get; set; } // Primary Key

        public string? FirstName { get; set; } 

        public string? LastName { get; set; } 

        public string? Email { get; set; } 

        public string? PhoneNumber { get; set; } 

  
        public string? Address { get; set; } 

        public string? OrganizationName { get; set; } // If donating on behalf of an organization

        public decimal? DonationAmount { get; set; } // Donation amount in CAD

  
        public string? MessageToRecipient { get; set; } 

        public DateTime? DonationDate { get; set; } = DateTime.UtcNow; // Timestamp of the donation

        public string? PreferredRegion { get; set; } // Preferred province or city to support (e.g., Ontario, Toronto)

        public bool? IsRecurringDonor { get; set; } // Indicates if the donor intends to make recurring contributions

        public bool IsActive { get; set; } = true; // Default to true (active)
    }
}
