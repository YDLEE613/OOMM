namespace BootcampTraineeBLL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTraineeDBObjects.Util;
    using BootcampTraineeBLL.Util;
    using BootcampTraineeDAL;
    using System.Net.Mail;

    /// <summary>
    /// This class manages calling methods in DAL, and getting and returning values fom DAL
    /// </summary>
    public class RestaurantBLL
    {
        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all active restaurants from database
        /// </summary>
        /// <returns>list of all active restaurants </returns>
        public List<RestaurantDBO> GetAllActiveRestaurants()
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of restaurant
            List<RestaurantDBO> lRestaurantList = lRestaurantDAL.GetAllActiveRestaurants();

            return lRestaurantList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to check if a restaurant serves sandwich or not.
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If the restaurant serves sandwich, return true. Otherwise, false</returns>
        public bool GetBoolSandwichByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get bool result
            bool lResult = lRestaurantDAL.GetBoolSandwichByRestaurantID(id);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all food items served in a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of food items</returns>
        public List<FoodItemDBO> GetAllFoodItemsByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of food items
            List<FoodItemDBO> lFoodItemsList = lRestaurantDAL.GetAllFoodItemsByRestaurantID(id);

            return lFoodItemsList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all entrees served in a restaurant that does not serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of entrees</returns>
        public List<FoodItemDBO> GetAllEntreeByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of entree items
            List<FoodItemDBO> lEntreeList = lRestaurantDAL.GetAllEntreeByRestaurantID(id);

            return lEntreeList;
        }

        /// <summary>
        /// Description: This method calls a method to retrieve all sides served in a restaurant that does not serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of sides</returns>
        public List<FoodItemDBO> GetAllSidesByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of side items
            List<FoodItemDBO> lFoodList = lRestaurantDAL.GetAllSideByRestaurantID(id);

            return lFoodList;

        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all beverages served in a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of beverages</returns>
        public List<FoodItemDBO> GetAllBeveragesByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of beverage items
            List<FoodItemDBO> lBeverageList = lRestaurantDAL.GetAllBeveragesByRestaurantID(id);

            return lBeverageList;

        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all cheeses served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of cheeses</returns>
        public List<FoodItemDBO> GetAllCheeseByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of cheese items
            List<FoodItemDBO> lCheeseList = lRestaurantDAL.GetAllCheeseByRestaurantID(id);

            return lCheeseList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all meats served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of meats</returns>
        public List<FoodItemDBO> GetAllMeatByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of meat items
            List<FoodItemDBO> lMeatList = lRestaurantDAL.GetAllMeatByRestaurantID(id);

            return lMeatList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all breads served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of breads</returns>
        public List<FoodItemDBO> GetAllBreadByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of bread items
            List<FoodItemDBO> lBreadList = lRestaurantDAL.GetAllBreadByRestaurantID(id);

            return lBreadList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all veggies served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of veggies</returns>
        public List<FoodItemDBO> GetAllVeggieByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of Veggie items
            List<FoodItemDBO> lFoodList = lRestaurantDAL.GetAllVeggieByRestaurantID(id);

            return lFoodList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all condiments served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of condiments</returns>
        public List<FoodItemDBO> GetAllCondimentByRestaurantByID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list of Condiment items
            List<FoodItemDBO> lFoodList = lRestaurantDAL.GetAllCondimentByRestaurantID(id);

            return lFoodList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>Restaurant object with data</returns>
        public RestaurantDBO FindRestaurantByRestaurantID(int id)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the restaurant
            RestaurantDBO lRestaurant = lRestaurantDAL.FindRestaurantByRestaurantID(id);

            return lRestaurant;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to insert user order into database
        /// </summary>
        /// <param name="iFoodItemsListUtil">Object that has data about user order and restaurant</param>
        /// <returns>If successfully inserted, return a unique user order id. Otherwise, 0</returns>
        public int CreateUserOrder(FoodItemsListUtil iFoodItemListUtil)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Insert into database
            int lUserOrderIDPK = lRestaurantDAL.CreateUserOrder(iFoodItemListUtil);

            return lUserOrderIDPK;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve the total price of food items
        /// </summary>
        /// <param name="iFoodItemIDPKsString">string of ids for food item</param>
        /// <returns>total price of food items</returns>
        public int CreateUserLineOrder(UserLineOrder iUserLineOrder)
        {
            // Instantiate objects
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();
                
            // Get Total price of items ordered if it is not a sandwich restaurant
            if (iUserLineOrder.IsSandwichRestaurant != 1)
            {
                iUserLineOrder.OrderPrice = lRestaurantDAL.GetTotalPriceByFoodItemIDs(iUserLineOrder.FoodItemsIDString);
            }

            // Insert into database
            int lResult = lRestaurantDAL.CreateUserLineOrder(iUserLineOrder);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all orders ordered and food items selected by a user
        /// </summary>
        /// <param name="id">A unique user id</param>
        /// <returns>List of orders and food items</returns>
        public List<UserOrderDBO> GetAllOrdersByUserID(int id)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get the list frmo database
            List<UserOrderDBO> lOrderList = lRestaurantDAL.GetAllOrdersByUserID(id);

            return lOrderList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about food items selected in an order
        /// </summary>
        /// <param name="iOrderID">A unique order id</param>
        /// <returns>List of food items selected</returns>
        public List<FoodItemDBO> GetAllFoodItemsByOrderID(int id)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get the list
            List<FoodItemDBO> lFoodItemList = lRestaurantDAL.GetAllFoodItemsByOrderID(id);

            return lFoodItemList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about all restaurants that, if search string is provided, has certain search string as part of any fields 
        /// </summary>
        /// <param name="searchString">specific texts that a user wants</param>
        /// <returns>List of restaurants</returns>
        public List<RestaurantDBO> GetAllRestaurants(string searchString)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get the list
            List<RestaurantDBO> lRestaurantList = lRestaurantDAL.GetAllRestaurants(searchString);

            return lRestaurantList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to insert data about a restaurant into database
        /// </summary>
        /// <param name="iRestaurant">Restaurant object with data</param>
        /// <returns>A unique restaurant id</returns>
        public int CreateRestaurant(RestaurantDBO iRestaurant)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get Inserted.RestaurantIDPK as return value
            int lRestaurantIDPK = lRestaurantDAL.CreateRestaurant(iRestaurant);

            return lRestaurantIDPK;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to update a restaurant with new values
        /// </summary>
        /// <param name="iRestaurant">Restaurant to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateRestaurantByRestaurantID(RestaurantDBO iRestaurant)
        {
            // Instantiate DAL Object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // Get bool result 
            bool lResult = lRestaurantDAL.UpdateRestaurantByRestaurantID(iRestaurant);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to remove a restaurant from database
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveRestaurantByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get result of bool 
            bool lResult = lRestaurantDAL.RemoveRestaurantByRestaurantID(id);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about all orders and food items selected from a restaurant within a week 
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of orders and food items selected for each order</returns>
        public List<UserOrderDBO> GetAllRecentOrdersByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get the list of user orders
            List<UserOrderDBO> lUserOrderList = lRestaurantDAL.GetAllRecentOrdersByRestaurantID(id);

            return lUserOrderList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about all ratings for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>list of ratings</returns>
        public List<UserOrderRatingDBO> GetAllRatingByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get list of user ratings
            List<UserOrderRatingDBO> lRatingList = lRestaurantDAL.GetAllRatingByRestaurantID(id);

            return lRatingList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to remove a food item from database
        /// </summary>
        /// <param name="id">A unique food item id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveFoodItemByFoodItemID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get result of bool value
            bool lResult = lRestaurantDAL.RemoveFoodItemByFoodItemID(id);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method to retrieve all food types
        /// </summary>
        /// <returns>list of food types</returns>
        public List<FoodTypeDBO> GetAllFoodTypes()
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get list of food types
            List<FoodTypeDBO> lFoodTypeList = lRestaurantDAL.GetAllFoodTypes();

            return lFoodTypeList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve all food ingredients for a sandwich
        /// </summary>
        /// <returns>list of ingredients</returns>
        public List<IngredientTypeDBO> GetAllFoodIngredients()
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get list of ingredient types
            List<IngredientTypeDBO> lIngredientList = lRestaurantDAL.GetAllFoodIngredients();

            return lIngredientList;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve average rating for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>Average rating</returns>
        public RestaurantRating GetRatingAverageByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get average rating
            RestaurantRating lRating = lRestaurantDAL.GetRatingAverageByRestaurantID(id);

            return lRating;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to insert food item into database
        /// </summary>
        /// <param name="iFoodItemDBO">Food item to be inserted</param>
        /// <returns>If successfully inserted, return a unique food item id. Otherwise, 0</returns>
        public int CreateFoodItem(FoodItemDBO iFoodItemDBO)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // a unique food item id
            int lFoodItemIDPK = lRestaurantDAL.CreateFoodItem(iFoodItemDBO);

            return lFoodItemIDPK;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about a food item
        /// </summary>
        /// <param name="id">a unique food item id</param>
        /// <returns>a food item with data </returns>
        public FoodItemDBO FindFoodItemByFoodItemID(int id)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // food item object
            FoodItemDBO lFoodItemDBO = lRestaurantDAL.FindFoodItemByFoodItemID(id);

            return lFoodItemDBO;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to update a food item with new values
        /// </summary>
        /// <param name="iFoodItemDBO">food item to be updated</param>
        /// <returns>if successfully updated, return true. Otherwise, false</returns>
        public bool UpdateFoodItemByFoodItemID(FoodItemDBO iFoodItemDBO)
        {
            // Instantiate DAL object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get result of bool value
            bool lResult = lRestaurantDAL.UpdateFoodItemByFoodItemID(iFoodItemDBO);

            return lResult;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to check if a user has ordered from a restaurant this week
        /// </summary>
        /// <param name="iUserIDPK">A unique user id</param>
        /// <param name="iRestaurantIDPK">A unique restaurant id</param>
        /// <returns>if user already orderd this week, return false. Otherwise, true</returns>
        public bool FindOrderCountThisWeekByUserIDAndRestaurantID(int iUserIDPK, int iRestaurantIDPK) // UserIDPK, RestaurantIDPK
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();
            
            // get result of bool value
            bool lResult = lRestaurantDAL.FindOrderCountThisWeekByUserIDAndRestaurantID(iUserIDPK, iRestaurantIDPK);

            return lResult;
        }

        /// <summary>
        /// Description: This method calculates weekly payments to restaurants
        /// </summary>
        /// <returns>Object with data</returns>
        public MeaningfulCalcUtil GetAllCalc()
        {
            // Instantiate objects
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();
            Calculation lCalculation = new Calculation();
            MeaningfulCalcUtil lCalcUtil = new MeaningfulCalcUtil();

            // populate restaurant list
            //List<RestaurantDBO> RestaurantList = this.GetAllRestaurants("");
            // Get average ratings for all restaurants
            //lCalcUtil.RatingList = new List<double>();
            //foreach (RestaurantDBO each in RestaurantList.Where(item => item.IsActive == 1).ToList())
            //{
            //    lCalcUtil.RatingList.Add(each.AverageRating);
            //}

            // Get all weeks since the oldest registration date of active restaurant
            lCalcUtil.WeekList = this.GetAllWeeks();

            // Get All Active restaurant
            lCalcUtil.RestaurantList = this.GetAllRestaurants("").Where(restaurant => restaurant.IsActive == 1).ToList();

            // Get All food items ordered from the date (WeekList[0][0] is the first date)
            List<UserOrderDBO> lFoodOrderList = lRestaurantDAL.GetAllFoodItemsOrderedByDate(lCalcUtil.WeekList[0][0]);

            // Get Total for each restaurant
            lCalcUtil.WeeklyTotal = this.GetTotalForEachRestaurant(lCalcUtil.RestaurantList, lFoodOrderList, lCalcUtil.WeekList);

            return lCalcUtil;
        }

        /// <summary>
        /// Description: This method get all weeks starting from oldest registration week among current active restaurants
        /// </summary>
        /// <returns>list of array of DateTime</returns>
        public List<DateTime[]> GetAllWeeks()
        {
            // Instantiate objects
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();
            Calculation lCalculation = new Calculation();

            // retrieve the oldest registration week of active restaurants
            DateTime[] lWeek = lRestaurantDAL.FindActiveRestaurantOldestRegistrationWeek();

            // calculate every week from oldest week until current week
            List<DateTime[]> lWeekList = lCalculation.GetAllWeeks(lWeek);

            return lWeekList;
        }

        /// <summary>
        /// Description: This method calculates weekly payment to each restaurant
        /// </summary>
        /// <param name="iRestaurantList">List of active restaurants</param>
        /// <param name="iFoodItemsOrderedList">List of user orders</param>
        /// <param name="iWeekList">List of weeks</param>
        /// <returns>string array [week index, restaurant index]</returns>
        public string[,] GetTotalForEachRestaurant(List<RestaurantDBO> iRestaurantList, List<UserOrderDBO> iFoodItemsOrderedList, List<DateTime[]> iWeekList) 
        {
            // Instantiate object
            Calculation lCalculation = new Calculation();

            // calculate the weekly payment
            string[,] WeeklyTotal = lCalculation.GetTotalForEachRestaurant(iRestaurantList, iFoodItemsOrderedList, iWeekList);

            return WeeklyTotal;
        }

        /// <summary>
        /// Description: This method calls a method in DAL to retrieve data about top 20 ratings and food items for each rating for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>list of order ratings</returns>
        public List<UserOrderRatingDBO> GetTopRatedOrdersByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // retrieve list of user ratings
            List<UserOrderRatingDBO> lUserOrderRatingList = lRestaurantDAL.GetTopRatedOrdersByRestaurantID(id);

            return lUserOrderRatingList;
        }

        /// <summary>
        /// Description: This method retrieves the delivery day for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If successfully retrieved, return a number corresponding to a day of week. Otherwise, -1</returns>
        public int FindDayOfDelieveryByRestaurantID(int id)
        {
            // Instantiate object
            RestaurantDAL lRestaurantDAL = new RestaurantDAL();

            // get the day of week
            int lDayOfWeek = lRestaurantDAL.FindDayOfDelieveryByRestaurantID(id);

            return lDayOfWeek;
        }

        /// <summary>
        /// Description: This method sends orders to restaurant email
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If successfully sent, return true, otherwise false</returns>
        public bool sendOrdersByRestaurantID(int id)
        {
            string email = FindRestaurantByRestaurantID(id).Email;
            List<UserOrderDBO> userOrders = GetAllRecentOrdersByRestaurantID(id);
            bool isSent = sendOrders(email, userOrders);

            return isSent;
        }

        /// <summary>
        /// Description: Helper method for sendOrdersByRestaurantID
        /// </summary>
        /// <param name="email">Restaurant Email</param>
        /// <param name="userOrders">list of user orders</param>
        /// <returns>IF successfully sent, return true. Otherwise false</returns>
        public bool sendOrders(string email, List<UserOrderDBO> userOrders) {
            bool isSent = false;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydmah1130@gmail.com");
                mail.To.Add(email);
                mail.Subject = generateSubject();
                mail.Body = getOrderDetails(userOrders);

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydmah1130@gmail.com", "8001Donn!");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                isSent = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return isSent;
        }

        /// <summary>
        /// Description: This method retrieves orders and make them into string
        /// </summary>
        public string getOrderDetails(List<UserOrderDBO> userOrders)
        {
            string orders = "";

            foreach(UserOrderDBO each in userOrders){
                orders += "Name: " + each.UserName + "\n"
                       + "Note: " + each.UserNote + "\n"
                       + "Orders: " + getFoodItems(each.FoodItemList)+ "\n"
                       + "Date: " + each.DateOrdered +"\n\n";
            }

            return orders;
        }

        /// <summary>
        /// Description: This method returns list of items as string
        /// </summary>
        public string getFoodItems(List<FoodItemDBO> foodItems)
        {
            string items = "";

            foreach(FoodItemDBO each in foodItems)
            {
                items += each.FoodItemName + ", ";
            }

            return items;
        }
            
        /// <summary>
        /// Description: This method returns subject for email
        /// </summary>
        public string generateSubject()
        {
            return DateTime.Today + " order from Onshore Bootcamp";
        }
    }
}
