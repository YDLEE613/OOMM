namespace BootcampTrainee.Models
{
    using System.Collections.Generic;

    public class BoardAndCommentsViewModel
    {
        public Board Board { get; set; } 
        public BoardComment BoardComment { get; set; }
        public List<BoardComment> BoardCommentList { get; set; }

    }
}