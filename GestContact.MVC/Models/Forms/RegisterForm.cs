using GestContact.MVC.Inftrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestContact.MVC.Models.Forms
{
    public class RegisterForm
    {
        [Required]
        [StringLength(50, MinimumLength=1)]
        public string Nom { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Prenom { get; set; }
        [Required]
        [StringLength(320, MinimumLength = 6)]
        [EmailAddress]
        [CheckEmail]
        public string Email { get; set; }
        [DisplayName("Mot de passe")]
        [Required]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression("^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$")]
        [DataType(DataType.Password)]
        public string Passwd { get; set; }
        [DisplayName("Confirmation")]
        [Required] 
        [Compare(nameof(Passwd))]
        [DataType(DataType.Password)]
        public string Passwd2 { get; set; }
    }
}
