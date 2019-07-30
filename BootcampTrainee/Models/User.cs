namespace BootcampTrainee.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using BootcampTrainee.CustomDataAnnotation;
    using BootcampTrainee.Models.Util;
    using BootcampTrainee.Models.SubModel;

    public class User : Common
    {
        public int UserIDPK { get; set; }

        [Required]
        [Display(Name = "User LogIn ID")]
        [MaxLength(50, ErrorMessage = "User ID is too long.")]
        [RegularExpression(@"^[^\s\,]+$", ErrorMessage = "User ID should not have a space.")]
        public string UserLogInID { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?#&])[A-Za-z\d$@$!%*?#&]{8,20}", 
                            ErrorMessage = "Password must have 8 ~ 20 characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string UserPassword { get; set; }


        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "Confirmation Password and Password should match.")]
        public string ConfirmPassword { get; set; }



        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "First Name is too long.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should contain only letters.")]
        public string UserFirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Last Name is too long.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should contain only letters.")]
        public string UserLastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange("", ErrorMessage = "Value for {0} must be before {2}")]
        public DateTime UserBirth { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
                            ErrorMessage = "Not in proper format")]
        public string UserEmail { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string UserPhone { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int UserRoleIDFK { get; set; }
        public string UserRoleName { get; set; }
        public List<Role> RoleList { get; set; }


        public DateTime UserDateCreated { get; set; }
        public DateTime UserDateModified { get; set; }

        
        [Display(Name = "Status")]
        public int UserIsActive { get; set; }
    }
}