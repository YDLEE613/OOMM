namespace BootcampTrainee.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using BootcampTrainee.Models.Util;


    public class BoardComment : Common
    {
        public int BoardCommentIDPK { get; set; }

        [Required(ErrorMessage ="Please Fill the content.")]
        [AllowHtml]
        public string Content { get; set; }
        public int BoardIDFK { get; set; }
        public int UserIDFK { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}