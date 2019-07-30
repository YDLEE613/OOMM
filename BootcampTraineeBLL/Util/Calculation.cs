using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampTraineeDAL;
using BootcampTraineeDBObjects;

namespace BootcampTraineeBLL.Util
{
    public class Calculation
    {
        /// <summary>
        /// Description: This method calculates and populate every week from start week to current week
        /// </summary>
        /// <param name="iDateStart">starting week</param>
        /// <returns>list of datetime [start, end]</returns>
        public List<DateTime[]> GetAllWeeks(DateTime[] iDateStart)
        {
            // iDateStart 
            // [0]: start date of the week, 
            // [1]: end date of the week
            // [2]: start date of this week, 
            // [3]: end date of this week
            DateTime lStartDate = iDateStart[0];
            DateTime lEndDate = iDateStart[1];
            DateTime lStartCurrDate = iDateStart[2];
            DateTime lEndCurrDate = iDateStart[3];

            // list to store every week
            List<DateTime[]> lDateList = new List<DateTime[]>();

            // add the first date
            lDateList.Add(new DateTime[] { lStartDate, lEndDate });

            // Add every week from the start week to curr week
            while (lStartDate < lStartCurrDate && lEndDate < lEndCurrDate)
            {
                DateTime[] eachWeek = new DateTime[2];

                // add a week both to start and end date
                lStartDate = lStartDate.AddDays(7);
                lEndDate = lEndDate.AddDays(7);

                // set values
                eachWeek[0] = lStartDate;
                eachWeek[1] = lEndDate;

                lDateList.Add(eachWeek);
            }

            return lDateList;
        }

        /// <summary>
        /// Description: This method calculates weekly payment to each restaurant
        /// </summary>
        /// <param name="iRestaurantList">List of active restaurants</param>
        /// <param name="iFoodItemsOrderedList">List of user orders</param>
        /// <param name="iWeekList">List of weeks</param>
        /// <returns>string array [week index, restaurant index]</returns>
        public string[,] GetTotalForEachRestaurant(List<RestaurantDBO> iRestaurantList, List<UserOrderDBO> iFoodOrderList, List<DateTime[]> iWeekList)
        {

            // store weekly payment to each restaurant
            string[,] WeeklyTotal = new string[iWeekList.Count, iRestaurantList.Count];

            // set dates
            DateTime lFirstWeekStart = iWeekList[0][0];
            DateTime lFirstWeekEnd = iWeekList[0][1];
            DateTime lLastWeekStart = iWeekList[iWeekList.Count - 1][0];
            DateTime lLastWeekEnd = iWeekList[iWeekList.Count - 1][0];


            // for each week, find food items from user orders for each restaurant and adds up the payment
            for (int wIndex = 0; wIndex < iWeekList.Count; wIndex++)
            {
                // for each restaurant, adds up the payment for orders
                for (int rIndex = 0; rIndex < iRestaurantList.Count; rIndex++)
                {
                    // variable to store weekly total
                    decimal lTotalTemp = 0;

                    // add up the cost of orders from each restaurant
                    foreach (UserOrderDBO userOrder in iFoodOrderList)
                    {
                        // bool value if an order is within a specific week
                        bool OrderedWithinWeek = (DateTime.Compare(iWeekList[wIndex][0], userOrder.DateOrdered) <= 0 &&
                                                 DateTime.Compare(userOrder.DateOrdered, iWeekList[wIndex][1]) <= 0);

                        //if an order is ordered from a restaurant within a specific week, add to total
                        if (OrderedWithinWeek && iRestaurantList[rIndex].RestaurantIDPK == userOrder.RestaurantIDFK)
                        {
                            lTotalTemp += userOrder.OrderPrice;
                        }
                    }

                    // store weekly sum for each restaurant
                    WeeklyTotal[wIndex, rIndex] = lTotalTemp.ToString();
                }
            }

            return WeeklyTotal;
        }
    }
}
