namespace BootcampTraineeDBObjects.Util
{
    using System;
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;

    public class MeaningfulCalcUtil
    {
        // weeks from min(date_created)
        public List<DateTime[]> WeekList { get; set; }

        // only active restaurant
        public List<RestaurantDBO> RestaurantList { get; set; }

        // only ratings of active restaurant
        public List<double> RatingList { get; set; }

        public string[,] WeeklyTotal { get; set; }
    }
}
