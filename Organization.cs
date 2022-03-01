using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationPortal.Models
{
    public class Organization
    {
        [Key]

        public int OrganizationId { get; set; }

        public string OrganizationName {get; set;}

        public string OrganizationDescription {get; set;}

        public string PostDescription { get; set; }

        public int Goal { get; set; }

        public String ImageUrl {get; set;}

        public int UserId { get; set; }
        public User Creator { get; set; }

        public int AmountCollected {get; set;}

        public int ZipCode {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}