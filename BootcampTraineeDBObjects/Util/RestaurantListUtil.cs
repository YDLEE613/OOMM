namespace BootcampTraineeDBObjects.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RestaurantListUtil
    {
        public RestaurantListUtil()
        {
            PageSize = 15;
        }
        public List<RestaurantDBO> RestaurantList { get; set; }
        public int PageSize { get; set; }
    }
}
