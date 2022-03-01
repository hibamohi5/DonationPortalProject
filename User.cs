using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.RegularExpressions;

namespace DonationPortal.Models
{
    public class User
    {

        [Key]

        public int UserId { get; set; }

        // --------------------------------------------NAME--------------------------------------------------
        [Required(ErrorMessage = "You must enter a name.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = " Name should be letters and spaces only.")]
        [MinLength(2, ErrorMessage = "Name should be only letters and spaces")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        // ---------------------------------------------------------------------------------------------------------

        // ---------------------------------------------EMAIL-------------------------------------------------------
        [Required(ErrorMessage = "You must provide your email.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }
        // ---------------------------------------------------------------------------------------------------------

        // -----------------------------------------PASSWORD------------------------------------------------------
        [Required(ErrorMessage = "You must provide a password.")]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters or longer!")]
        // the line below is for not showing up the password (characters) on screen
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // ---------------------------------------------------------------------------------------------------------

        // ----------------------------------------CONFIRM PASSWORD---------------------------------------------------
        [NotMapped]

        // the line below is validation for your confirm password
        [Compare("Password", ErrorMessage = "The Confirm Password does not match!")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
        // ---------------------------------------------------------------------------------------------------------
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public int Amount {get; set;}

        public int PledgeYear {get; set;}
        //To be able to know the creator of the idea so we can compare
        // the creator to the user
        public List<Organization> CreatedOrganization {get; set;}

    }
}