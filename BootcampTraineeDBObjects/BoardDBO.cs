namespace BootcampTraineeDBObjects
{
    using System;

    public class BoardDBO
    {
        public int BoardIDPK { get; set; }
        public int UserIDFK { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int IsFixed { get; set; }
        public int CategoryIDFK { get; set; }
        public string CategoryName { get; set; }


    }
}
