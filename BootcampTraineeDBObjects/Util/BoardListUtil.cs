namespace BootcampTraineeDBObjects.Util
{
    using BootcampTraineeDBObjects.SubDBO;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BoardListUtil : Common
    {
        public BoardListUtil()
        {
            PageSize = 15;
        }

        public List<BoardDBO> BoardList { get; set; }
        public CategoryDBO Category { get; set; }
        public SelectList CategoryList { get; set; }
        public int PageSize { get; set; }
    }
}
