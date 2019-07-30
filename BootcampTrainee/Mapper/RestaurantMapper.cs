namespace BootcampTrainee.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using BootcampTrainee.Models.Util;
    using BootcampTrainee.Models.SubModel;
    using BootcampTrainee.Models;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.Util;
    using BootcampTraineeDBObjects.SubDBO;

    /// <summary>
    /// This class manages mapping from an object or a list of objects to another object or list of another objects
    /// </summary>
    public class RestaurantMapper
    {
        /// <summary>
        /// Description: This method maps database object to Model object
        /// </summary>
        /// <param name="iFoodItemsListUtil">Object that has data about entrees, sides, beverages or choices for sandwich</param>
        /// <returns>Model Object with entrees, sides, beverages or choices for sandwich</returns>
        public FoodItemsListUtilModel MapDBOToModel(FoodItemsListUtil iFoodItemsListUtil)
        {
            FoodItemsListUtilModel lUtilModel = new FoodItemsListUtilModel();

            // set values for model object
            lUtilModel.RestaurantIDFK = iFoodItemsListUtil.Restaurant.RestaurantIDPK;
            lUtilModel.RestaurantName = iFoodItemsListUtil.Restaurant.RestaurantName;
            lUtilModel.IsSandwichRestaurant = iFoodItemsListUtil.Restaurant.IsSandwichRestaurant;
            lUtilModel.RestaurantNotice = iFoodItemsListUtil.Restaurant.Notice;
            lUtilModel.PerSandwichPrice = iFoodItemsListUtil.Restaurant.SandwichPrice;

            // for a restaurant that does not offer sandwich
            if (lUtilModel.IsSandwichRestaurant == 0)
            {
                // For restaurants that do not offer sandwich
                lUtilModel.EntreeList = new List<FoodItem>();
                lUtilModel.SideList = new List<FoodItem>();
                lUtilModel.BeverageList = new List<FoodItem>();

                // populate list for entree, side and beverage
                // map each entree
                foreach (FoodItemDBO each in iFoodItemsListUtil.EntreeList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;

                    lUtilModel.EntreeList.Add(lFood);
                }


                // map each side
                foreach (FoodItemDBO each in iFoodItemsListUtil.SideList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;

                    lUtilModel.SideList.Add(lFood);
                }

                // map each beverage
                foreach (FoodItemDBO each in iFoodItemsListUtil.BeverageList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;

                    lUtilModel.BeverageList.Add(lFood);
                }
            }
            else
            {
                // For restaurants that offer sandwich
                lUtilModel.CheeseList = new List<FoodItem>();
                lUtilModel.MeatList = new List<FoodItem>();
                lUtilModel.BreadList = new List<FoodItem>();
                lUtilModel.VeggieList = new List<FoodItem>();
                lUtilModel.CondimentList = new List<FoodItem>();

                // populate list for cheese, meat, bread, veggie, and condiment
                // map each Cheese
                foreach (FoodItemDBO each in iFoodItemsListUtil.CheeseList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;

                    lUtilModel.CheeseList.Add(lFood);
                }

                // map each Meat
                foreach (FoodItemDBO each in iFoodItemsListUtil.MeatList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;

                    lUtilModel.MeatList.Add(lFood);
                }

                // map each Bread
                foreach (FoodItemDBO each in iFoodItemsListUtil.BreadList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;
                    lUtilModel.BreadList.Add(lFood);
                }

                // map each Veggie
                foreach (FoodItemDBO each in iFoodItemsListUtil.VeggieList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;
                    lUtilModel.VeggieList.Add(lFood);
                }

                // map each Condiment
                foreach (FoodItemDBO each in iFoodItemsListUtil.CondimentList)
                {
                    FoodItem lFood = new FoodItem();

                    // set values
                    lFood.FoodItemIDPK = each.FoodItemIDPK;
                    lFood.FoodItemName = each.FoodItemName;
                    lFood.IsSelected = each.IsSelected;
                    lUtilModel.CondimentList.Add(lFood);
                }
            }

            return lUtilModel;
        }

        /// <summary>
        /// Description: This method maps Restaurant database object to Restaurant Model object
        /// </summary>
        /// <param name="iRestaurantDBO">Restaurant database object with data</param>
        /// <returns>Restaurant Model object</returns>
        public Restaurant MapDBOToModel(RestaurantDBO iRestaurantDBO)
        {
            Restaurant lRestaurant = new Restaurant();

            // set values
            lRestaurant.RestaurantIDPK = iRestaurantDBO.RestaurantIDPK;
            lRestaurant.RestaurantName = iRestaurantDBO.RestaurantName;
            lRestaurant.DayofWeek = iRestaurantDBO.DayofWeek;
            lRestaurant.Contact = iRestaurantDBO.Contact;
            lRestaurant.Notice = iRestaurantDBO.Notice;
            lRestaurant.DateCreated = iRestaurantDBO.DateCreated;
            lRestaurant.DateModified = iRestaurantDBO.DateModified;
            lRestaurant.IsActive = iRestaurantDBO.IsActive;
            lRestaurant.IsSandwichRestaurant = iRestaurantDBO.IsSandwichRestaurant;
            lRestaurant.SandwichPrice = iRestaurantDBO.SandwichPrice;

            return lRestaurant;
        }

        /// <summary>
        /// Description: This method maps list of food type database object to list of food type Model object
        /// </summary>
        /// <param name="iFoodTypeDBOList">List of food type database objects</param>
        /// <returns>List of food type model objects</returns>
        public List<FoodType> MapDBOToModel(List<FoodTypeDBO> iFoodTypeDBOList)
        {
            // list to stored food types
            List<FoodType> lFoodTypeList = new List<FoodType>();

            // map each food type
            foreach (FoodTypeDBO each in iFoodTypeDBOList)
            {
                FoodType lFoodType = new FoodType();

                // set values
                lFoodType.FoodTypeIDPK = each.FoodTypeDBOIDPK;
                lFoodType.FoodTypeName = each.FoodTypeDBOName;

                lFoodTypeList.Add(lFoodType);
            }

            return lFoodTypeList;
        }

        /// <summary>
        /// Description: This method maps list of ingredient type database object to list of ingredient model objects
        /// </summary>
        /// <param name="iIngredientTypeDBOList">List of ingredient type database objects</param>
        /// <returns>List of ingredient type Model objects </returns>
        public List<IngredientType> MapDBOToModel(List<IngredientTypeDBO> iIngredientTypeDBOList)
        {
            // list to store ingredient types
            List<IngredientType> lIngredientList = new List<IngredientType>();

            // map each database object to model object
            foreach (IngredientTypeDBO each in iIngredientTypeDBOList)
            {
                IngredientType lIngredientType = new IngredientType();

                // set values
                lIngredientType.IngredientTypeIDPK = each.IngredientTypeDBOIDPK;
                lIngredientType.IngredientTypeName = each.IngredientTypeDBOName;

                lIngredientList.Add(lIngredientType);
            }

            return lIngredientList;

        }

        /// <summary>
        /// Description: This method maps food item database object to food item Model object
        /// </summary>
        /// <param name="iFoodItemDBO">Food item database object</param>
        /// <returns>food item Model object</returns>
        public FoodItem MapDBOToModel(FoodItemDBO iFoodItemDBO)
        {
            FoodItem lFoodItem = new FoodItem();

            // set values
            lFoodItem.FoodItemIDPK = iFoodItemDBO.FoodItemIDPK;
            lFoodItem.FoodItemName = iFoodItemDBO.FoodItemName;
            lFoodItem.FoodTypeSelected = iFoodItemDBO.FoodTypeIDFK;
            lFoodItem.RestaurantIDFK = iFoodItemDBO.RestaurantIDFK;
            lFoodItem.IsSandwichRestaurant = iFoodItemDBO.IsSandwichRestaurant;
            lFoodItem.IngredientTypeSelected = iFoodItemDBO.IngredientTypeIDFK;
            lFoodItem.Description = iFoodItemDBO.Description;
            lFoodItem.Price = iFoodItemDBO.Price;

            return lFoodItem;
        }

        /// <summary>
        /// Description: This method maps Model object to database object
        /// </summary>
        /// <param name="iFoodItemsListUtil">Model Object with entrees, sides, beverages or choices for sandwich</param>
        /// <returns>Database Object that has data about entrees, sides, beverages or choices for sandwich</returns>
        public FoodItemsListUtil MapModelToDBO(FoodItemsListUtilModel iUtilModel)
        {
            // object to be returned
            FoodItemsListUtil lFoodItemsList = new FoodItemsListUtil();

            // map data about restaurant
            lFoodItemsList.RestaurantIDFK = iUtilModel.RestaurantIDFK;
            lFoodItemsList.RestaurantName = iUtilModel.RestaurantName;
            lFoodItemsList.IsSandwichRestaurant = iUtilModel.IsSandwichRestaurant;
            lFoodItemsList.SandwichPrice = iUtilModel.PerSandwichPrice;
            lFoodItemsList.UserIDFK = iUtilModel.UserIDFK;
            lFoodItemsList.UserNote = iUtilModel.UserNote;

            // Is NOT sandwich restaurant
            if (iUtilModel.IsSandwichRestaurant == 0)
            {

                // store Selected sides
                lFoodItemsList.SideList = new List<FoodItemDBO>();

                // map entree
                lFoodItemsList.EntreeID = iUtilModel.EntreeID;
                lFoodItemsList.EntreeName = iUtilModel.EntreeName;


                // check for null
                if (iUtilModel.SideList != null)
                {
                    // map each side chosen
                    foreach (FoodItem each in iUtilModel.SideList.Where(foodItem => foodItem.IsSelected == true).ToList())
                    {
                        FoodItemDBO lFoodItemDBO = new FoodItemDBO();

                        //set values
                        lFoodItemDBO.FoodItemIDPK = each.FoodItemIDPK;
                        lFoodItemDBO.FoodItemName = each.FoodItemName;
                        lFoodItemDBO.IsSelected = each.IsSelected;

                        lFoodItemsList.SideList.Add(lFoodItemDBO);
                    }
                }

                // map Beverage
                lFoodItemsList.BeverageID = iUtilModel.BeverageID;
                lFoodItemsList.BeverageName = iUtilModel.BeverageName;
            }
            else
            {
                // lists to stored selected ones
                lFoodItemsList.VeggieList = new List<FoodItemDBO>();
                lFoodItemsList.CondimentList = new List<FoodItemDBO>();

                // map cheese
                lFoodItemsList.CheeseID = iUtilModel.CheeseID;
                lFoodItemsList.CheeseName = iUtilModel.CheeseName;

                // map meat
                lFoodItemsList.MeatID = iUtilModel.MeatID;
                lFoodItemsList.MeatName = iUtilModel.MeatName;

                // map bread
                lFoodItemsList.BreadID = iUtilModel.BreadID;
                lFoodItemsList.BreadName = iUtilModel.BreadName;

                // check for null
                if (iUtilModel.VeggieList != null)
                {
                    // map each veggie chosen
                    foreach (FoodItem each in iUtilModel.VeggieList.Where(veggie => veggie.IsSelected == true).ToList())
                    {
                        FoodItemDBO lFoodItemDBO = new FoodItemDBO();

                        // set values
                        lFoodItemDBO.FoodItemIDPK = each.FoodItemIDPK;
                        lFoodItemDBO.FoodItemName = each.FoodItemName;
                        lFoodItemDBO.IsSelected = each.IsSelected;

                        lFoodItemsList.VeggieList.Add(lFoodItemDBO);
                    }
                }

                // check for null
                if (iUtilModel.CondimentList != null)
                {

                    // map each condiment chosen
                    foreach (FoodItem each in iUtilModel.CondimentList.Where(condiment => condiment.IsSelected == true).ToList())
                    {
                        FoodItemDBO lFoodItemDBO = new FoodItemDBO();

                        // set values
                        lFoodItemDBO.FoodItemIDPK = each.FoodItemIDPK;
                        lFoodItemDBO.FoodItemName = each.FoodItemName;
                        lFoodItemDBO.IsSelected = each.IsSelected;

                        lFoodItemsList.CondimentList.Add(lFoodItemDBO);
                    }
                }
            }

            return lFoodItemsList;
        }

        /// <summary>
        /// Description: This method maps Restaurant Model object to Restaurant Database object
        /// </summary>
        /// <param name="iRestaurant">Restaurant Model object</param>
        /// <returns>Restaurant Database object</returns>
        public RestaurantDBO MapModelToDBO(Restaurant iRestaurant)
        {
            RestaurantDBO lRestaurantDBO = new RestaurantDBO();

            // set values
            lRestaurantDBO.RestaurantIDPK = iRestaurant.RestaurantIDPK;
            lRestaurantDBO.RestaurantName = iRestaurant.RestaurantName;
            lRestaurantDBO.DayofWeek = iRestaurant.DayofWeek;
            lRestaurantDBO.Contact = iRestaurant.Contact;
            lRestaurantDBO.Notice = iRestaurant.Notice;
            lRestaurantDBO.IsActive = iRestaurant.IsActive;
            lRestaurantDBO.IsSandwichRestaurant = iRestaurant.IsSandwichRestaurant;
            lRestaurantDBO.SandwichPrice = iRestaurant.SandwichPrice;

            return lRestaurantDBO;
        }

        /// <summary>
        /// Description: This method maps Fooditem Model object to Fooditem database object
        /// </summary>
        /// <param name="iFoodItem"></param>
        /// <returns></returns>
        public FoodItemDBO MapModelToDBO(FoodItem iFoodItem)
        {
            FoodItemDBO lFoodItemDBO = new FoodItemDBO();

            // set values
            lFoodItemDBO.FoodItemIDPK = iFoodItem.FoodItemIDPK;
            lFoodItemDBO.FoodItemName = iFoodItem.FoodItemName;
            lFoodItemDBO.FoodTypeIDFK = iFoodItem.FoodTypeSelected;
            lFoodItemDBO.IngredientTypeIDFK = iFoodItem.IngredientTypeSelected;
            lFoodItemDBO.Description = iFoodItem.Description;
            lFoodItemDBO.RestaurantIDFK = iFoodItem.RestaurantIDFK;
            lFoodItemDBO.Price = iFoodItem.Price;
            return lFoodItemDBO;
        }


    }
}