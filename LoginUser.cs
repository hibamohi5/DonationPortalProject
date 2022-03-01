using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationPortal.Models
{
    public class LoginUser
    {

        // ---------------------------------------------EMAIL-------------------------------------------------------
        [Required(ErrorMessage = "You must provide your email.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email")]
        [Display(Name = "Email")]
        public string LoginEmail { get; set; }
// ---------------------------------------------------------------------------------------------------------

// -----------------------------------------PASSWORD------------------------------------------------------
        [Required(ErrorMessage = "You must provide a password.")]
        // the line below is for not showing up the password (characters) on screen
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }

// ---------------------------------------------------------------------------------------------------------


    }
}