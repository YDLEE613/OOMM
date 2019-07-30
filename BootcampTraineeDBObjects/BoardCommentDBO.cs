namespace BootcampTraineeDBObjects
{
    using System;

    public class BoardCommentDBO
    {
        public int BoardCommentIDPK { get; set; }
        public int UserIDFK { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public string Content { get; set; }
        public int BoardIDFK { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
       
    }
}
