namespace BootcampTraineeDAL
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTraineeDBObjects.Util;

    /// <summary>
    /// Description: This class manages database connection and CRUD on database tables especially regarding restaurants
    /// </summary>
    public class RestaurantDAL
    {
        // connection string to database
        //string lConnectionString = "Server=LAPTOP-VC1M94MC\\SQLEXPRESS01;Database=OnshoreCapstone;Trusted_Connection=True;";
        private string lConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        /// <summary>
        /// Description: This method retrieves all active restaurants from database
        /// </summary>
        /// <returns>list of all active restaurants </returns>
        public List<RestaurantDBO> GetAllActiveRestaurants()
        {
            // list to be returned
            List<RestaurantDBO> lRestaurantList = new List<RestaurantDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use store procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllActiveRestaurants", lConn))
                    {
                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all restaurants, and add each restaurant to the list
                            while (lReader.Read())
                            {
                                // Instantiate a restaurant
                                RestaurantDBO lRestaurantDBO = new RestaurantDBO();

                                // set values
                                lRestaurantDBO.RestaurantIDPK = (int)lReader["restaurant_id"];
                                lRestaurantDBO.RestaurantName = (string)lReader["restaurant_name"];
                                lRestaurantDBO.DayofWeek = (string)lReader["day_of_week"];
                                lRestaurantDBO.Contact = (string)lReader["contact"];
                                lRestaurantDBO.DateCreated = (DateTime)lReader["date_created"];
                                lRestaurantDBO.DateModified = (DateTime)lReader["date_modified"];
                                lRestaurantDBO.IsActive = Convert.ToInt32(lReader["is_active"]);
                                lRestaurantDBO.IsSandwichRestaurant = Convert.ToInt32(lReader["is_sandwich_restaurant"]);

                                // check for DB null values
                                lRestaurantDBO.AverageRating = lReader["average"] == DBNull.Value ? 0 : Math.Round(((double)lReader["average"]), 1);
                                lRestaurantDBO.Notice = lReader["notice"] == DBNull.Value ? null : (string)lReader["notice"];

                                // add to the list to be returned
                                lRestaurantList.Add(lRestaurantDBO);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lRestaurantList;
        }

        /// <summary>
        /// Description: This method checks if a restaurant serves sandwich or not.
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If the restaurant serves sandwich, return true. Otherwise, false</returns>
        public bool GetBoolSandwichByRestaurantID(int id)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetBoolSandwichByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // set return value based on query result
                            while (lReader.Read())
                            {
                                int IsSandwich = Convert.ToInt32(lReader["is_sandwich_restaurant"]);

                                if (IsSandwich == 1)
                                {
                                    lResult = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lResult;
        }

        /// <summary>
        /// Description: This method retrieves all food items served in a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of food items</returns>
        public List<FoodItemDBO> GetAllFoodItemsByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lFoodItemList = null;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllFoodItemsByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {

                            lFoodItemList = new List<FoodItemDBO>();

                            // retrieve data for each food ite and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lFoodItem = new FoodItemDBO();

                                // set values
                                lFoodItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lFoodItem.FoodItemName = (string)lReader["food_item_name"];
                                lFoodItem.FoodTypeIDFK = (int)lReader["food_type_id_FK"];
                                lFoodItem.FoodTypeName = (string)lReader["food_type_name"];
                                lFoodItem.RestaurantIDFK = (int)lReader["restaurant_id_FK"];

                                // Check for DBNull value
                                if (lReader["ingredient_type_id_FK"] == DBNull.Value)
                                {
                                    lFoodItem.IngredientTypeIDFK = 0;
                                    lFoodItem.IngredientTypeName = "";
                                }
                                else
                                {
                                    lFoodItem.IngredientTypeIDFK = (int)lReader["ingredient_type_id_FK"];
                                    lFoodItem.IngredientTypeName = (string)lReader["ingredient_type"];
                                }

                                lFoodItem.Description = lReader["food_description"] == DBNull.Value ? "" : (string)lReader["food_description"];
                                lFoodItem.Price = lReader["food_price"] == DBNull.Value ? 0 : Convert.ToDecimal(lReader["food_price"]);

                                lFoodItemList.Add(lFoodItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lFoodItemList;
        }

        /// <summary>
        /// Description: This method retrieves all entrees served in a restaurant that does not serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of entrees</returns>
        public List<FoodItemDBO> GetAllEntreeByRestaurantID(int id)
        {
            // List to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllEntreeByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each entree and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lEachItem = new FoodItemDBO();

                                // set values
                                lEachItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lEachItem.FoodItemName = (string)lReader["food_item_name"];
                                lEachItem.Price = (decimal)lReader["food_price"];

                                lList.Add(lEachItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all sides served in a restaurant that does not serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of sides</returns>
        public List<FoodItemDBO> GetAllSideByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllSidebyRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each side and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lFood = new FoodItemDBO();

                                // set values
                                lFood.FoodItemIDPK = (int)lReader["food_item_id"];
                                lFood.FoodItemName = (string)lReader["food_item_name"];
                                lFood.Price = (Decimal)lReader["food_price"];
                                lFood.IsSelected = false;

                                lList.Add(lFood);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all beverages served in a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of beverages</returns>
        public List<FoodItemDBO> GetAllBeveragesByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllBeveragesByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each beverage and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lEachItem = new FoodItemDBO();

                                // set values
                                lEachItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lEachItem.FoodItemName = (string)lReader["food_item_name"];
                                lEachItem.Price = (Decimal)lReader["food_price"];

                                lList.Add(lEachItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all cheeses served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of cheeses</returns>
        public List<FoodItemDBO> GetAllCheeseByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllCheeseByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each cheese and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lEachItem = new FoodItemDBO();

                                // set values
                                lEachItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lEachItem.FoodItemName = (string)lReader["food_item_name"];

                                lList.Add(lEachItem);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all meats served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of meats</returns>
        public List<FoodItemDBO> GetAllMeatByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllMeatByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each meat and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lEachItem = new FoodItemDBO();

                                // set values
                                lEachItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lEachItem.FoodItemName = (string)lReader["food_item_name"];

                                lList.Add(lEachItem);

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all breads served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of breads</returns>
        public List<FoodItemDBO> GetAllBreadByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllBreadByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each bread and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lEachItem = new FoodItemDBO();

                                // set values
                                lEachItem.FoodItemIDPK = (int)lReader["food_item_id"];
                                lEachItem.FoodItemName = (string)lReader["food_item_name"];

                                lList.Add(lEachItem);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all veggies served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of veggies</returns>
        public List<FoodItemDBO> GetAllVeggieByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllVeggieByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();


                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each veggie and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lFood = new FoodItemDBO();

                                // set values
                                lFood.FoodItemName = (string)lReader["food_item_name"];
                                lFood.FoodItemIDPK = (int)lReader["food_item_id"];
                                lFood.IsSelected = false;

                                lList.Add(lFood);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;
        }

        /// <summary>
        /// Description: This method retrieves all condiments served in a restaurant that serve sandwich
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of condiments</returns>
        public List<FoodItemDBO> GetAllCondimentByRestaurantID(int id)
        {
            // list to be returned
            List<FoodItemDBO> lList = new List<FoodItemDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllCondimentByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each condiment and add to the list
                            while (lReader.Read())
                            {
                                FoodItemDBO lFood = new FoodItemDBO();

                                // set values
                                lFood.FoodItemName = (string)lReader["food_item_name"];
                                lFood.FoodItemIDPK = (int)lReader["food_item_id"];
                                lFood.IsSelected = false;

                                lList.Add(lFood);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lList;

        }

        /// <summary>
        /// Description: This method retrieves data about a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>Restaurant object with data</returns>
        public RestaurantDBO FindRestaurantByRestaurantID(int id)
        {
            // object to be returned
            RestaurantDBO lRestaurantDBO = new RestaurantDBO();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindRestaurantByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about the restaurant
                            while (lReader.Read())
                            {
                                // set values
                                lRestaurantDBO.RestaurantIDPK = (int)lReader["restaurant_id"];
                                lRestaurantDBO.RestaurantName = (string)lReader["restaurant_name"];
                                lRestaurantDBO.DayofWeek = (string)lReader["day_of_week"];
                                lRestaurantDBO.Contact = (string)lReader["contact"];
                                lRestaurantDBO.DateCreated = (DateTime)lReader["date_created"];
                                lRestaurantDBO.DateModified = (DateTime)lReader["date_modified"];
                                lRestaurantDBO.IsActive = Convert.ToInt32(lReader["is_active"]);
                                lRestaurantDBO.IsSandwichRestaurant = Convert.ToInt32(lReader["is_sandwich_restaurant"]);

                                // check for DB null values
                                lRestaurantDBO.Notice = lReader["notice"] == DBNull.Value ? "" : lRestaurantDBO.Notice = (string)lReader["notice"];
                                lRestaurantDBO.SandwichPrice = lReader["sandwich_price"] == DBNull.Value ? default : Convert.ToDecimal(lReader["sandwich_price"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lRestaurantDBO;
        }

        /// <summary>
        /// Description: This method inserts user order into database
        /// </summary>
        /// <param name="iFoodItemsListUtil">Object that has data about user order and restaurant</param>
        /// <returns>If successfully inserted, return a unique user order id. Otherwise, 0</returns>
        public int CreateUserOrder(FoodItemsListUtil iFoodItemsListUtil)
        {
            // return value
            int lResult = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateUserOrder", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id_FK", SqlDbType.Int).Value = iFoodItemsListUtil.UserIDFK;
                        lComm.Parameters.AddWithValue("@parm_restaurant_id_FK", SqlDbType.Int).Value = iFoodItemsListUtil.RestaurantIDFK;

                        // check null or emptry values
                        if (string.IsNullOrEmpty(iFoodItemsListUtil.UserNote))
                        {
                            lComm.Parameters.AddWithValue("@parm_note", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_note", SqlDbType.VarChar).Value = iFoodItemsListUtil.UserNote;
                        }

                        // check null or emptry values
                        if (iFoodItemsListUtil.IsSandwichRestaurant == 1)
                        {

                            lComm.Parameters.AddWithValue("@parm_order_price", SqlDbType.Decimal).Value = iFoodItemsListUtil.SandwichPrice;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_order_price", SqlDbType.Int).Value = DBNull.Value;
                        }

                        // open connection
                        lConn.Open();

                        // execute query and get order id
                        lResult = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method inserts all food items selected into database
        /// </summary>
        /// <param name="iUserLineOrder">Object that has data about food items user selected</param>
        /// <returns>If successfully inserted, return a number of rows affected. Otherwise, 0</returns>
        public int CreateUserLineOrder(UserLineOrder iUserLineOrder)
        {
            // return value
            int lResult = 0;

            try
            {
                // establish connection string
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateUserLineOrder", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_order_id_FK", SqlDbType.Int).Value = iUserLineOrder.UserOrderIDFK;
                        lComm.Parameters.AddWithValue("@parm_user_choices", SqlDbType.VarChar).Value = iUserLineOrder.FoodItemsIDString;
                        lComm.Parameters.AddWithValue("@parm_order_price", SqlDbType.Decimal).Value = iUserLineOrder.OrderPrice;

                        // open connection
                        lConn.Open();
                        
                        // execute query and get return value
                        lResult = lComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method retrieves all orders ordered and food items selected by a user
        /// </summary>
        /// <param name="id">A unique user id</param>
        /// <returns>List of orders and food items</returns>
        public List<UserOrderDBO> GetAllOrdersByUserID(int id)
        {
            // list to be returned
            List<UserOrderDBO> lOrderList = new List<UserOrderDBO>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllOrdersByUserID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about user order
                            while (lReader.Read())
                            {
                                // Instantiate object
                                UserOrderDBO lUserOrder = new UserOrderDBO();

                                // set values
                                lUserOrder.UserOrderIDPK = (int)lReader["user_order_id"];
                                lUserOrder.UserIDFK = (int)lReader["user_id"];
                                lUserOrder.DateOrdered = (DateTime)lReader["date_ordered"];

                                // check null values
                                lUserOrder.RestaurantIDFK = lReader["restaurant_id"] == DBNull.Value ? 0 : (int)lReader["restaurant_id"];
                                lUserOrder.RestaurantName = lReader["restaurant_name"] == DBNull.Value ? "Deleted Restaurant" : (string)lReader["restaurant_name"];
                                lUserOrder.UserNote = lReader["note"] == DBNull.Value ? "" : (string)lReader["note"];
                                lUserOrder.OrderRating = lReader["score"] == DBNull.Value ? 0 : (double)lReader["score"];

                                // Retrieve all food items selected for an order
                                lUserOrder.FoodItemList = this.GetAllFoodItemsByOrderID(lUserOrder.UserOrderIDPK);

                                // Add to list
                                lOrderList.Add(lUserOrder);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lOrderList;
        }

        /// <summary>
        /// Description: This method retrieves data about food items selected in an order
        /// </summary>
        /// <param name="iOrderID">A unique order id</param>
        /// <returns>List of food items selected</returns>
        public List<FoodItemDBO> GetAllFoodItemsByOrderID(int iOrderID)
        {
            // list to be returned
            List<FoodItemDBO> lFoodItemList = new List<FoodItemDBO>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllFoodItemsByOrderID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_order_id_FK", SqlDbType.Int).Value = iOrderID;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Retrieve data about all food items selected
                            while (lReader.Read())
                            {
                                FoodItemDBO lFoodItem = new FoodItemDBO();

                                // check db null values
                                lFoodItem.FoodItemIDPK = lReader["food_items_FK"] == DBNull.Value ? 0 : (int)lReader["food_items_FK"];
                                lFoodItem.FoodItemName = lReader["food_item_name"] == DBNull.Value ? "Deleted Item" : (string)lReader["food_item_name"];
                                lFoodItem.FoodTypeIDFK = lReader["food_type_id"] == DBNull.Value ? 0 : (int)lReader["food_type_id"];
                                lFoodItem.FoodTypeName = lReader["food_type_name"] == DBNull.Value ? "" : (string)lReader["food_type_name"];

                                lFoodItemList.Add(lFoodItem);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lFoodItemList;
        }

        /// <summary>
        /// Description: This method retrieves data about all restaurants that, if search string is provided, has certain search string as part of any fields 
        /// </summary>
        /// <param name="searchString">specific texts that a user wants</param>
        /// <returns>List of restaurants</returns>
        public List<RestaurantDBO> GetAllRestaurants(string searchString)
        {
            // list to be returned
            List<RestaurantDBO> lRestaurantList = new List<RestaurantDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllRestaurants", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // check if string is null or empty
                        if (!string.IsNullOrEmpty(searchString))
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = searchString;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = DBNull.Value;
                        }

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all restaurants
                            while (lReader.Read())
                            {

                                RestaurantDBO lRestaurantDBO = new RestaurantDBO();

                                // set values
                                lRestaurantDBO.RestaurantIDPK = (int)lReader["restaurant_id"];
                                lRestaurantDBO.RestaurantName = (string)lReader["restaurant_name"];
                                lRestaurantDBO.DayofWeek = (string)lReader["day_of_week"];
                                lRestaurantDBO.Contact = (string)lReader["contact"];
                                lRestaurantDBO.DateCreated = (DateTime)lReader["date_created"];
                                lRestaurantDBO.DateModified = (DateTime)lReader["date_modified"];
                                lRestaurantDBO.IsActive = Convert.ToInt32(lReader["is_active"]);
                                lRestaurantDBO.IsSandwichRestaurant = Convert.ToInt32(lReader["is_sandwich_restaurant"]);
                                lRestaurantDBO.AverageRating = lReader["average"] == DBNull.Value ? 0 : Math.Round(((double)lReader["average"]), 1);
                                
                                // check for DB null values
                                lRestaurantDBO.Notice = lReader["notice"] == DBNull.Value ? null : (string)lReader["notice"];
                                lRestaurantDBO.SandwichPrice = lReader["sandwich_price"] == DBNull.Value ? default : Convert.ToDecimal(lReader["sandwich_price"]);

                                lRestaurantList.Add(lRestaurantDBO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lRestaurantList;
        }

        /// <summary>
        /// Description: This method inserts data about a restaurant into database
        /// </summary>
        /// <param name="iRestaurant">Restaurant object with data</param>
        /// <returns>A unique restaurant id</returns>
        public int CreateRestaurant(RestaurantDBO iRestaurant)
        {
            // return value
            int lResult = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateRestaurant", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_name", SqlDbType.Int).Value = iRestaurant.RestaurantName;
                        lComm.Parameters.AddWithValue("@parm_day_of_week", SqlDbType.VarChar).Value = iRestaurant.DayofWeek;
                        lComm.Parameters.AddWithValue("@parm_contact", SqlDbType.VarChar).Value = iRestaurant.Contact;
                        lComm.Parameters.AddWithValue("@parm_is_active", SqlDbType.Bit).Value = iRestaurant.IsActive;
                        lComm.Parameters.AddWithValue("@parm_is_sandwich_restaurant", SqlDbType.Int).Value = iRestaurant.IsSandwichRestaurant;

                        // check for null or empty value
                        if (!string.IsNullOrEmpty(iRestaurant.Notice))
                        {
                            lComm.Parameters.AddWithValue("@parm_notice", SqlDbType.VarChar).Value = iRestaurant.Notice;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_notice", SqlDbType.VarChar).Value = DBNull.Value;
                        }


                        if (iRestaurant.SandwichPrice == default)
                        {
                            lComm.Parameters.AddWithValue("@parm_sandwich_price", SqlDbType.Decimal).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_sandwich_price", SqlDbType.Decimal).Value = iRestaurant.SandwichPrice;
                        }
                         
                        // open connection
                        lConn.Open();

                        // get restaurant id as return value
                        lResult = Convert.ToInt32(lComm.ExecuteScalar());

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method updates a restaurant with new values
        /// </summary>
        /// <param name="iRestaurant">Restaurant to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateRestaurantByRestaurantID(RestaurantDBO iRestaurant)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_UpdateRestaurantByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id_PK", SqlDbType.Int).Value = iRestaurant.RestaurantIDPK;
                        lComm.Parameters.AddWithValue("@parm_restaurant_name", SqlDbType.VarChar).Value = iRestaurant.RestaurantName;
                        lComm.Parameters.AddWithValue("@parm_day_of_week", SqlDbType.VarChar).Value = iRestaurant.DayofWeek;
                        lComm.Parameters.AddWithValue("@parm_contact", SqlDbType.VarChar).Value = iRestaurant.Contact;
                        lComm.Parameters.AddWithValue("@parm_is_active", SqlDbType.Bit).Value = iRestaurant.IsActive;
                        lComm.Parameters.AddWithValue("@parm_is_sandwich_restaurant", SqlDbType.Bit).Value = iRestaurant.IsSandwichRestaurant;
                        lComm.Parameters.AddWithValue("@parm_sandwich_price", SqlDbType.Decimal).Value = iRestaurant.SandwichPrice;

                        // pass DB null if string is null or empty
                        if (string.IsNullOrEmpty(iRestaurant.Notice))
                        {
                            lComm.Parameters.AddWithValue("@parm_notice", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_notice", SqlDbType.VarChar).Value = iRestaurant.Notice;
                        }

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method removes a restaurant from database
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveRestaurantByRestaurantID(int id)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_RemoveRestaurantByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for store procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;

        }

        /// <summary>
        /// Description: This method retrieves data about all orders and food items selected from a restaurant within a week 
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>List of orders and food items selected for each order</returns>
        public List<UserOrderDBO> GetAllRecentOrdersByRestaurantID(int id) // last 6 days
        {
            // list to be returned
            List<UserOrderDBO> lUserOrderList = new List<UserOrderDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllRecentOrdersByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Retrieve data about each order and food items selected for each order
                            while (lReader.Read())
                            {
                                UserOrderDBO lUserOrderDBO = new UserOrderDBO();

                                // set values
                                lUserOrderDBO.UserOrderIDPK = (int)lReader["user_order_id"];
                                lUserOrderDBO.UserIDFK = (int)lReader["user_id_FK"];
                                lUserOrderDBO.UserName = (string)lReader["user_name"];
                                lUserOrderDBO.RestaurantIDFK = (int)lReader["restaurant_id_FK"];
                                lUserOrderDBO.RestaurantName = (string)lReader["restaurant_name"];
                                lUserOrderDBO.DateOrdered = (DateTime)lReader["date_ordered"];
                                lUserOrderDBO.OrderPrice = (decimal)lReader["order_price"];

                                // check db null value
                                lUserOrderDBO.UserNote = lReader["note"] == DBNull.Value ? null : (string)lReader["note"];

                                // Get the list of food items for each order by UserOrderIDPK
                                lUserOrderDBO.FoodItemList = this.GetAllFoodItemsByOrderID(lUserOrderDBO.UserOrderIDPK);

                                lUserOrderList.Add(lUserOrderDBO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lUserOrderList;
        }

        /// <summary>
        /// Description: This method retrieves data about all ratings for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>list of ratings</returns>
        public List<UserOrderRatingDBO> GetAllRatingByRestaurantID(int id)
        {
            // list to be returned
            List<UserOrderRatingDBO> lRatingList = new List<UserOrderRatingDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllRatingByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all ratings for a restaurant and add to the list
                            while (lReader.Read())
                            {
                                UserOrderRatingDBO lUserRating = new UserOrderRatingDBO();

                                // set values
                                lUserRating.UserOrderRatingIDPK = (int)lReader["user_order_rating_id"];
                                lUserRating.UserOrderIDFK = (int)lReader["user_order_id_FK"];
                                lUserRating.DateCreated = (DateTime)lReader["rating_date_created"];
                                lUserRating.Score = (double)lReader["score"];
                                lUserRating.UserID = (int)lReader["user_id_FK"];
                                lUserRating.UserName = (string)lReader["user_name"];

                                // check db null value
                                lUserRating.Content = lReader["content"] == DBNull.Value ? "" : (string)lReader["content"];

                                lRatingList.Add(lUserRating);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lRatingList;
        }

        /// <summary>
        /// Description: This method removes a food item from database
        /// </summary>
        /// <param name="id">A unique food item id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveFoodItemByFoodItemID(int id)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_RemoveFoodItemByFoodItemID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_food_item_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method retrieves all food types
        /// </summary>
        /// <returns>list of food types</returns>
        public List<FoodTypeDBO> GetAllFoodTypes()
        {
            // list to be returned
            List<FoodTypeDBO> lFoodTypeList = new List<FoodTypeDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllFoodTypes", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all food types and add to list
                            while (lReader.Read())
                            {
                                FoodTypeDBO lFoodType = new FoodTypeDBO();

                                // set values
                                lFoodType.FoodTypeDBOIDPK = (int)lReader["food_type_id"];
                                lFoodType.FoodTypeDBOName = (string)lReader["food_type_name"];

                                lFoodTypeList.Add(lFoodType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lFoodTypeList;
        }

        /// <summary>
        /// Description: This method retrieves all food ingredients for a sandwich
        /// </summary>
        /// <returns>list of ingredients</returns>
        public List<IngredientTypeDBO> GetAllFoodIngredients()
        {
            // list to be returned
            List<IngredientTypeDBO> lIngredientList = new List<IngredientTypeDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllFoodIngredients", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all ingredients and add to list
                            while (lReader.Read())
                            {
                                IngredientTypeDBO lIngredientType = new IngredientTypeDBO();

                                // set values
                                lIngredientType.IngredientTypeDBOIDPK = (int)lReader["ingredient_id"];
                                lIngredientType.IngredientTypeDBOName = (string)lReader["ingredient_type"];

                                lIngredientList.Add(lIngredientType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lIngredientList;
        }

        /// <summary>
        /// Description: This method retrieves average rating for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>Average rating</returns>
        public RestaurantRating GetRatingAverageByRestaurantID(int id)
        {
            // object to be returned
            RestaurantRating RestaurantRating = new RestaurantRating();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetRatingAverageByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about average rating
                            while (lReader.Read())
                            {
                                RestaurantRating.RatingAverage = (double)lReader["average"];
                                RestaurantRating.RatingCount = (int)lReader["count"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return RestaurantRating;
        }

        /// <summary>
        /// Description: This method inserts food item into database
        /// </summary>
        /// <param name="iFoodItemDBO">Food item to be inserted</param>
        /// <returns>If successfully inserted, return a unique food item id. Otherwise, 0</returns>
        public int CreateFoodItem(FoodItemDBO iFoodItemDBO)
        {
            // return value
            int lFoodItemIDPK = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateFoodItem", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_food_type_id_FK", SqlDbType.Int).Value = iFoodItemDBO.FoodTypeIDFK;
                        lComm.Parameters.AddWithValue("@parm_restaurant_id_FK", SqlDbType.Int).Value = iFoodItemDBO.RestaurantIDFK;
                        lComm.Parameters.AddWithValue("@parm_food_item_name", SqlDbType.VarChar).Value = iFoodItemDBO.FoodItemName;
                        lComm.Parameters.AddWithValue("@parm_price", SqlDbType.Decimal).Value = iFoodItemDBO.Price;

                        // if food item is not for sandwich, insert null.
                        if (iFoodItemDBO.IngredientTypeIDFK == 0)
                        {
                            lComm.Parameters.AddWithValue("@parm_ingredient_type_id_FK", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_ingredient_type_id_FK", SqlDbType.Int).Value = iFoodItemDBO.IngredientTypeIDFK;
                        }

                        // if description is empty, insert null
                        if (string.IsNullOrEmpty(iFoodItemDBO.Description))
                        {
                            lComm.Parameters.AddWithValue("@parm_description", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_description", SqlDbType.VarChar).Value = iFoodItemDBO.Description;
                        }

                        lConn.Open();

                        // execute and get a unique food item as return value
                        lFoodItemIDPK = Convert.ToInt32(lComm.ExecuteScalar());

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lFoodItemIDPK;
        }

        /// <summary>
        /// Description: This method retrieves data about a food item
        /// </summary>
        /// <param name="id">a unique food item id</param>
        /// <returns>a food item with data </returns>
        public FoodItemDBO FindFoodItemByFoodItemID(int id)
        {
            // object to be returned
            FoodItemDBO lFoodItemDBO = new FoodItemDBO();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindFoodItemByFoodItemID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_food_item_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about a food item
                            while (lReader.Read())
                            {
                                // set values
                                lFoodItemDBO.FoodItemIDPK = (int)lReader["food_item_id"];
                                lFoodItemDBO.FoodItemName = (string)lReader["food_item_name"];
                                lFoodItemDBO.FoodTypeIDFK = (int)lReader["food_type_id"];
                                lFoodItemDBO.FoodTypeName = (string)lReader["food_type_name"];
                                lFoodItemDBO.RestaurantIDFK = (int)lReader["restaurant_id_FK"];
                                lFoodItemDBO.IsSandwichRestaurant = Convert.ToInt32(lReader["is_sandwich_restaurant"]) == 1 ? true : false;

                                // check for db null values
                                lFoodItemDBO.Description = lReader["food_description"] == DBNull.Value ? "" : (string)lReader["food_description"];
                                lFoodItemDBO.Price = lReader["food_price"] == DBNull.Value ? default : (Decimal)(lReader["food_price"]);

                                if (lReader["ingredient_type"] == DBNull.Value)
                                {
                                    lFoodItemDBO.IngredientTypeIDFK = 0;
                                    lFoodItemDBO.IngredientTypeName = "";
                                }
                                else
                                {
                                    lFoodItemDBO.IngredientTypeIDFK = (int)lReader["ingredient_type_id_FK"];
                                    lFoodItemDBO.IngredientTypeName = (string)lReader["ingredient_type"];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lFoodItemDBO;
        }

        /// <summary>
        /// Description: This method updates a food item with new values
        /// </summary>
        /// <param name="iFoodItemDBO">food item to be updated</param>
        /// <returns>if successfully updated, return true. Otherwise, false</returns>
        public bool UpdateFoodItemByFoodItemID(FoodItemDBO iFoodItemDBO)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_UpdateFoodItemByFoodItemID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_food_item_id", SqlDbType.Int).Value = iFoodItemDBO.FoodItemIDPK;
                        lComm.Parameters.AddWithValue("@parm_food_item_name", SqlDbType.VarChar).Value = iFoodItemDBO.FoodItemName;
                        lComm.Parameters.AddWithValue("@parm_food_type_id_FK", SqlDbType.Int).Value = iFoodItemDBO.FoodTypeIDFK;

                        // if not an ingredient type, insert null
                        if (iFoodItemDBO.IngredientTypeIDFK == 0)
                        {
                            lComm.Parameters.AddWithValue("@parm_ingredient_type_id_FK", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_ingredient_type_id_FK", SqlDbType.Int).Value = iFoodItemDBO.IngredientTypeIDFK;
                        }

                        // if description is not provided, insert null
                        if (string.IsNullOrEmpty(iFoodItemDBO.Description))
                        {
                            lComm.Parameters.AddWithValue("@parm_food_description", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_food_description", SqlDbType.VarChar).Value = iFoodItemDBO.Description;
                        }

                        // if price is not provided, insert null
                        if (iFoodItemDBO.Price == default)
                        {
                            lComm.Parameters.AddWithValue("@parm_price", SqlDbType.Decimal).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_price", SqlDbType.Decimal).Value = iFoodItemDBO.Price;
                        }

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lResult;
        }

        /// <summary>
        /// Description: This method checks if a user has ordered from a restaurant this week
        /// </summary>
        /// <param name="iUserIDPK">A unique user id</param>
        /// <param name="iRestaurantIDPK">A unique restaurant id</param>
        /// <returns>if user already orderd this week, return false. Otherwise, true</returns>
        public bool FindOrderCountThisWeekByUserIDAndRestaurantID(int iUserIDPK, int iRestaurantIDPK)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindOrderCountThisWeekByUserIDAndRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = iUserIDPK;
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = iRestaurantIDPK;

                        // open connection
                        lConn.Open();

                        // if the count is 0, it means current user has not ordered from the restaurant this week. Set return value true.
                        if (Convert.ToInt32(lComm.ExecuteScalar()) > 0)
                        {
                            lResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            // if count is not 0, return default value (false).
            return lResult;

        }

        /// <summary>
        /// Description: This method retrieves the total price of food items
        /// </summary>
        /// <param name="iFoodItemIDPKsString">string of ids for food item</param>
        /// <returns>total price of food items</returns>
        public decimal GetTotalPriceByFoodItemIDs(string iFoodItemIDPKsString)
        {
            // return value
            decimal lTotalPrice = default;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetTotalPriceByFoodItemIDs", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_food_item_ids_string", SqlDbType.VarChar).Value = iFoodItemIDPKsString;

                        // open connection
                        lConn.Open();

                        // get total price
                        lTotalPrice = (decimal)lComm.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lTotalPrice;
        }

        /// <summary>
        /// Description: This method retrieves the oldest week of active restaurants
        /// </summary>
        /// <returns>an array of start date and end date of the week, and start date and end date of current week </returns>
        public DateTime[] FindActiveRestaurantOldestRegistrationWeek()
        {
            // number of data
            int count = 4;
            DateTime[] lWeek = new DateTime[count];

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindActiveRestaurantOldestRegistrationWeek", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // get dates
                            while (lReader.Read())
                            {
                                lWeek[0] = (DateTime)lReader["start_date"];
                                lWeek[1] = (DateTime)lReader["end_date"];
                                lWeek[2] = (DateTime)lReader["start_curr_date"];
                                lWeek[3] = (DateTime)lReader["end_curr_date"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lWeek;
        }

        /// <summary>
        /// Description: This method retrieves all orders that are ordered after certain date
        /// </summary>
        /// <param name="iStartDate">start date</param>
        /// <returns>list of user orders ordered from certain date</returns>
        public List<UserOrderDBO> GetAllFoodItemsOrderedByDate(DateTime iStartDate)
        {
            // list of user orders to be returned
            List<UserOrderDBO> lUserOrderList = new List<UserOrderDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllFoodItemsOrderedByDate", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_start_date", SqlDbType.DateTime).Value = iStartDate;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each order and add to the list
                            while (lReader.Read())
                            {
                                UserOrderDBO lUserOrderDBO = new UserOrderDBO();

                                // set values
                                lUserOrderDBO.UserOrderIDPK = (int)lReader["user_order_id"];
                                lUserOrderDBO.UserIDFK = (int)lReader["user_id_FK"];
                                lUserOrderDBO.RestaurantIDFK = (int)lReader["restaurant_id_FK"];
                                lUserOrderDBO.DateOrdered = (DateTime)lReader["date_ordered"];
                                lUserOrderDBO.OrderPrice = (decimal)lReader["order_price"];

                                lUserOrderList.Add(lUserOrderDBO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lUserOrderList;
        }

        /// <summary>
        /// Description: This method retrieves data about top 20 ratings and food items for each rating for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>list of order ratings</returns>
        public List<UserOrderRatingDBO> GetTopRatedOrdersByRestaurantID(int id)
        {
            // list to be returned
            List<UserOrderRatingDBO> lRatingList = new List<UserOrderRatingDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    //use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetTopRatedOrdersByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about each rating and food items for each rating, and add to the list
                            while (lReader.Read())
                            {
                                UserOrderRatingDBO lRating = new UserOrderRatingDBO();

                                // set values
                                lRating.UserOrderRatingIDPK = (int)lReader["user_order_rating_id"];
                                lRating.UserOrderIDFK = (int)lReader["user_order_id_FK"];
                                lRating.UserName = (string)lReader["user_name"];
                                lRating.Content = lReader["content"] == DBNull.Value ? "" : (string)lReader["content"];
                                lRating.Score = (double)lReader["score"];
                                lRating.DateCreated = (DateTime)lReader["date_created"];

                                // get the list of food items by order id
                                lRating.FoodItemList = this.GetAllFoodItemsByOrderID(lRating.UserOrderIDFK);

                                lRatingList.Add(lRating);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lRatingList;
        }

        /// <summary>
        /// Description: This method retrieves the delivery day for a restaurant
        /// </summary>
        /// <param name="id">A unique restaurant id</param>
        /// <returns>If successfully retrieved, return a number corresponding to a day of week. Otherwise, -1</returns>
        public int FindDayOfDelieveryByRestaurantID(int id)
        {
            // return value
            int lDayOfWeek = -1;
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindDayOfDelieveryByRestaurantID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_restaurant_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        // 0 : sunday
                        // ...
                        // 7 : saturday
                        lDayOfWeek = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lDayOfWeek;
        }
    }

}
