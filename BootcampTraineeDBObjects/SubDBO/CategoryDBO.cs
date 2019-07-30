namespace BootcampTraineeDBObjects.SubDBO
{
    public class CategoryDBO
    {
        public CategoryDBO(int iCategoryID, string iCategoryName)
        {
            this.CategoryID = iCategoryID;
            this.CategoryName = iCategoryName;
        }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
