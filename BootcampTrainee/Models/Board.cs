namespace BootcampTrainee.Models 
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using BootcampTrainee.Models.Util;

    public class Board : Common
    {
        public int BoardIDPK { get; set; }
        public int UserIDFK { get; set; }

        [Display(Name = "Author")]
        public string UserName { get; set; }
        public string UserRoleName { get; set; }

        [Required(ErrorMessage = "Title can't be emtpy.")]
        [MaxLength(255, ErrorMessage = "Title is too long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content can't be empty.")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        [Display(Name = "Written")]
        public DateTime DateModified { get; set; }

        [Required(ErrorMessage = "Please Choose one.")]
        public int IsFixed { get; set; }

        [Required(ErrorMessage = "Please Choose one.")]
        public int CategoryIDFK { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }


    }
}