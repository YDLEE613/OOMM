namespace BootcampTrainee.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using BootcampTrainee.Models;
    using BootcampTrainee.Models.Util;
    using BootcampTrainee.Models.SubModel;
    using BootcampTraineeBLL;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.Util;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTrainee.Mapper;
    using BootcampTrainee.Filters;
    using System.Net.Mail;

    /// <summary>
    /// This class manages GET and POST actions regarding Restaurant
    /// </summary>
    public class RestaurantController : Controller
    {
        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult Index()
        {
            // instantiate object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // Get list of restaurants
            List<RestaurantDBO> lRestaurantList = lRestaurantBLL.GetAllActiveRestaurants();

            return View(lRestaurantList);
        }


        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult OrderSelection(int id)
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lMapper = new RestaurantMapper();

            // current user id
            int lUserIDFK = Convert.ToInt32(Session["AUTHUserIDPK"]);

            // get delivery day for the restaurant
            int lDayOfWeek = lRestaurantBLL.FindDayOfDelieveryByRestaurantID(id);
            int today = (int)DateTime.Now.DayOfWeek;

            // Check if it is the first order for the restaurant of the current user this week
            bool AlreadyOrdered = lRestaurantBLL.FindOrderCountThisWeekByUserIDAndRestaurantID(lUserIDFK, id);

            // The user already ordered, return to restaurant index page with message
            if (AlreadyOrdered)
            {
                // message for user
                TempData["msg"] = "<script>alert('You have already ordered!');</script>";
                return RedirectToAction("Index", "Restaurant");
            }
            else if (lDayOfWeek != today)
            {
                if (lDayOfWeek > today)
            {
            // message for trying to order for future
            TempData["msg"] = "<script>alert('You cannot order today.');</script>";
            }
            else
            {
            // message for past day
            TempData["msg"] = "<script>alert('You can order next week.');</script>";
            }

                return RedirectToAction("Index", "Restaurant");
            }

            // Instantiate FoodItemListUtil object
            FoodItemsListUtil lFoodItemsListUtil = new FoodItemsListUtil();

            // Find if the restaurant offer sandwich
            lFoodItemsListUtil.Restaurant = lRestaurantBLL.FindRestaurantByRestaurantID(id);
            lFoodItemsListUtil.IsSandwichRestaurant = lFoodItemsListUtil.Restaurant.IsSandwichRestaurant;
            lFoodItemsListUtil.RestaurantIDFK = id;

            // populate menus for restaurants that do NOT offer sandwich
            if (lFoodItemsListUtil.IsSandwichRestaurant == 0)
            {
                // Entree List - Drop down
                lFoodItemsListUtil.EntreeList = lRestaurantBLL.GetAllEntreeByRestaurantID(id);

                // Side List - Check box
                lFoodItemsListUtil.SideList = lRestaurantBLL.GetAllSidesByRestaurantID(id);


                // Beverage List - Drop Down
                lFoodItemsListUtil.BeverageList = lRestaurantBLL.GetAllBeveragesByRestaurantID(id);
            }

            // populate menus for restaurants that do offer sandwich
            else
            {
                // Cheese List
                lFoodItemsListUtil.CheeseList = lRestaurantBLL.GetAllCheeseByRestaurantID(id);

                // Meat List
                lFoodItemsListUtil.MeatList = lRestaurantBLL.GetAllMeatByRestaurantID(id);

                // Bread List
                lFoodItemsListUtil.BreadList = lRestaurantBLL.GetAllBreadByRestaurantID(id);

                // Veggie List
                lFoodItemsListUtil.VeggieList = lRestaurantBLL.GetAllVeggieByRestaurantID(id);

                // Condiment List
                lFoodItemsListUtil.CondimentList = lRestaurantBLL.GetAllCondimentByRestaurantByID(id);
            }

            // Map database object to Model
            FoodItemsListUtilModel lListUtilModel = lMapper.MapDBOToModel(lFoodItemsListUtil);

            return View(lListUtilModel);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult OrderSelection(FoodItemsListUtilModel iFoodItemListUtil)
        {
            // instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lRestaurantMapper = new RestaurantMapper();

            // check if every input is valid and check if user chosed more than 1 food item
            if (ModelState.IsValid)
            {
                // map Model to database object
                FoodItemsListUtil lFoodItemsListUtil = lRestaurantMapper.MapModelToDBO(iFoodItemListUtil);

                // Instantiate UserLineOrder object
                UserLineOrder lUserLineOrder = new UserLineOrder();

                // Get user choices for restaurants that do not offer sandwich
                if (iFoodItemListUtil.IsSandwichRestaurant == 0)
                {
                    // count the number of sides chosen
                    int SideListCount = lFoodItemsListUtil.SideList.Where(s => s.IsSelected == true).ToList().Count;

                    // check if user chosed more than 1 food item
                    if (lFoodItemsListUtil.EntreeID == null && SideListCount == 0 && lFoodItemsListUtil.BeverageID == null)
                    {
                        TempData["msg"] = "<script>alert('Please choose at least 1 food item.');</script>";
                        return RedirectToAction("OrderSelection", "Restaurant", new { id = iFoodItemListUtil.RestaurantIDFK });
                    }


                    // limit number of sides to choose
                    if (SideListCount <= 2)
                    {
                        // create order and get the order id from the inserted row in UserOrder 
                        int lUserOrderIDPK = lRestaurantBLL.CreateUserOrder(lFoodItemsListUtil);

                        lUserLineOrder.UserOrderIDFK = lUserOrderIDPK;
                        lUserLineOrder.FoodItemsList = new List<FoodItemDBO>();

                        // add selected entree to the list
                        lUserLineOrder.FoodItemsList.Add(new FoodItemDBO(Convert.ToInt32(lFoodItemsListUtil.EntreeID), lFoodItemsListUtil.EntreeName));

                        // add selected sides to the list
                        foreach (FoodItemDBO each in lFoodItemsListUtil.SideList)
                        {
                            lUserLineOrder.FoodItemsList.Add(each);
                        }

                        // add selected beverage to the list
                        lUserLineOrder.FoodItemsList.Add(new FoodItemDBO(Convert.ToInt32(lFoodItemsListUtil.BeverageID), lFoodItemsListUtil.BeverageName));
                    }
                    else
                    {
                        // message for choosing too many sides
                        TempData["msg"] = "<script>alert('You should choose less than 2 sides');</script>";
                        return RedirectToAction("OrderSelection", "Restaurant", new { id = iFoodItemListUtil.RestaurantIDFK });
                    }
                }

                // get user choices for restaurants that do offer sandwich
                else
                {
                    // Get the user id from the inserted row in UserOrder 
                    int lUserOrderIDPK = lRestaurantBLL.CreateUserOrder(lFoodItemsListUtil);

                    // Insert into database (UserLineOrder) - UserOrderIDFK, FoodItemFK
                    lUserLineOrder.UserOrderIDFK = lUserOrderIDPK;
                    lUserLineOrder.FoodItemsList = new List<FoodItemDBO>();

                    // add selected cheese to the list
                    lUserLineOrder.FoodItemsList.Add(new FoodItemDBO(Convert.ToInt32(lFoodItemsListUtil.CheeseID), lFoodItemsListUtil.CheeseName));

                    // add selected meat to the list
                    lUserLineOrder.FoodItemsList.Add(new FoodItemDBO(Convert.ToInt32(lFoodItemsListUtil.MeatID), lFoodItemsListUtil.MeatName));

                    // add selected bread to the list
                    lUserLineOrder.FoodItemsList.Add(new FoodItemDBO(Convert.ToInt32(lFoodItemsListUtil.BreadID), lFoodItemsListUtil.BreadName));

                    // add selected veggies to the list
                    foreach (FoodItemDBO each in lFoodItemsListUtil.VeggieList)
                    {
                        lUserLineOrder.FoodItemsList.Add(each);
                    }

                    // add selected condiments to the list
                    foreach (FoodItemDBO each in lFoodItemsListUtil.CondimentList)
                    {
                        lUserLineOrder.FoodItemsList.Add(each);
                    }
                }

                // Append each food item id and name as string if foodItemsList is not emtory
                if (lUserLineOrder.FoodItemsList != null)
                {
                    foreach (FoodItemDBO each in lUserLineOrder.FoodItemsList)
                    {
                        lUserLineOrder.FoodItemsIDString += each.FoodItemIDPK + ",";
                        lUserLineOrder.FoodItemsNameString += each.FoodItemName + ",";
                    }

                    // set values
                    lUserLineOrder.OrderPrice = iFoodItemListUtil.PerSandwichPrice;
                    lUserLineOrder.IsSandwichRestaurant = iFoodItemListUtil.IsSandwichRestaurant;

                    // insert all food items selected into database
                    int lResult = lRestaurantBLL.CreateUserLineOrder(lUserLineOrder);

                    if (lResult > 0)
                    {
                        // redirect to order confirmation page
                        return RedirectToAction("OrderConfirm", new { UserLineOrder = lUserLineOrder.FoodItemsNameString });
                    }
                    else
                    {
                        // message on failure
                        TempData["msg"] = "<script>alert('Your Order is not submitted. Please try again.');</script>";
                    }
                }
            }
            else
            {
                // Model - not valid
                TempData["msg"] = "<script>alert('Please Fill all Required Information');</script>";
            }


            return RedirectToAction("Index", "Restaurant");
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult OrderConfirm(string UserLineOrder)
        {
            // split food items separated by comma
            string[] FoodList = UserLineOrder.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return View(FoodList);
        }


        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantList(string searchString)
        {
            // Instantitate BLL object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantListUtil lRestaurantUtil = new RestaurantListUtil();

            // Get the list of entrie restaurant
            lRestaurantUtil.RestaurantList = lRestaurantBLL.GetAllRestaurants(searchString);

            return View(lRestaurantUtil);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantRegister()
        {
            Restaurant lRestaurant = new Restaurant();

            // add days for delivery
            lRestaurant.DayList = new List<string>();
            lRestaurant.DayList.Add("Monday");
            lRestaurant.DayList.Add("Tuesday");
            lRestaurant.DayList.Add("Wednesday");
            lRestaurant.DayList.Add("Thursday");
            lRestaurant.DayList.Add("Friday");
            lRestaurant.DayList.Add("Saturday");
            lRestaurant.DayList.Add("Sunday");

            return View(lRestaurant);
        }

        [HttpPost]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantRegister(Restaurant iRestaurant)
        {
            if (ModelState.IsValid)
            {
                // Map Models.Restaurant to db objects
                RestaurantMapper lRestaurantMapper = new RestaurantMapper();
                RestaurantDBO lRestaurantDBO = lRestaurantMapper.MapModelToDBO(iRestaurant);

                // insert mapped object into database
                RestaurantBLL lRestaurantBLL = new RestaurantBLL();
                int lRestaurantIDPK = lRestaurantBLL.CreateRestaurant(lRestaurantDBO);


                if (lRestaurantIDPK > 0)
                {
                    // success
                    ViewBag.msg = "<script>alert('Registered Successfully');</script>";
                    return RedirectToAction("RestaurantList", "Restaurant");
                }
                else
                {
                    // failure
                    ViewBag.msg = "<script>alert('Registeration Failed');</script>";
                }
            }
            else
            {
                //not valid model state
                ViewBag.msg = "<script>alert('Not Valid Inputs');</script>";
            }

            // re-populate days to the list in case of failure
            iRestaurant.DayList = new List<string>();
            iRestaurant.DayList.Add("Monday");
            iRestaurant.DayList.Add("Tuesday");
            iRestaurant.DayList.Add("Wednesday");
            iRestaurant.DayList.Add("Thursday");
            iRestaurant.DayList.Add("Friday");
            iRestaurant.DayList.Add("Saturday");
            iRestaurant.DayList.Add("Sunday");

            return View(iRestaurant);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantProfile(int id, string searchString)
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lRestaurantMapper = new RestaurantMapper();

            // Instantiate util object
            RestaurantFoodsRatingUtil lRestaurantUtil = new RestaurantFoodsRatingUtil();

            // Get info about a restaurant by restaurant id
            lRestaurantUtil.Restaurant = lRestaurantBLL.FindRestaurantByRestaurantID(id);

            // Get list of food items by restaurant id
            lRestaurantUtil.FoodItemList = lRestaurantBLL.GetAllFoodItemsByRestaurantID(id);

            // check if searchstring not null
            if (!string.IsNullOrEmpty(searchString) && searchString != "reset")
            {
                lRestaurantUtil.FoodItemList = lRestaurantUtil.FoodItemList.Where(food => food.FoodItemName.ToLower().Contains(searchString) ||
                                                                                  food.FoodTypeName.ToLower().Contains(searchString) ||
                                                                                  food.IngredientTypeName.ToLower().Contains(searchString)).ToList();

                // set current tab
                ViewBag.tab = "nav-foods";
            }
            else
            {
                // if user reset search, populate food item list again
                lRestaurantUtil.FoodItemList = lRestaurantBLL.GetAllFoodItemsByRestaurantID(id);

                if (searchString == "reset")
                {
                    // set current tab
                    ViewBag.tab = "nav-foods";
                }
                else
                {
                    // set currert tab
                    ViewBag.tab = "nav-info";
                }

            }

            // Get list of rating for restaurant by id
            lRestaurantUtil.RatingList = lRestaurantBLL.GetAllRatingByRestaurantID(id);

            lRestaurantUtil.RestaurantRating = lRestaurantBLL.GetRatingAverageByRestaurantID(id);

            return View(lRestaurantUtil);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantUpdate(int id)
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lRestaurantMapper = new RestaurantMapper();

            // Get info about the restaurant
            RestaurantDBO lRestaurantDBO = lRestaurantBLL.FindRestaurantByRestaurantID(id);

            // map DB object to model
            Restaurant lRestaurant = lRestaurantMapper.MapDBOToModel(lRestaurantDBO);

            // Add days to the list
            lRestaurant.DayList = new List<string>();
            lRestaurant.DayList.Add("Monday");
            lRestaurant.DayList.Add("Tuesday");
            lRestaurant.DayList.Add("Wednesday");
            lRestaurant.DayList.Add("Thursday");
            lRestaurant.DayList.Add("Friday");
            lRestaurant.DayList.Add("Saturday");
            lRestaurant.DayList.Add("Sunday");

            return View(lRestaurant);
        }

        [HttpPost]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantUpdate(Restaurant iRestaurant)
        {
            if (ModelState.IsValid)
            {
                // validation for price
                if (!Decimal.TryParse(iRestaurant.SandwichPrice.ToString(), out decimal price))
                {
                    return View(iRestaurant);
                }
                // Instantiate objects
                RestaurantBLL lRestaurantBLL = new RestaurantBLL();
                RestaurantMapper lRestaurantMapper = new RestaurantMapper();

                // Map Model to DB object
                RestaurantDBO lRestaurantDBO = lRestaurantMapper.MapModelToDBO(iRestaurant);

                // get result after update
                bool lResult = lRestaurantBLL.UpdateRestaurantByRestaurantID(lRestaurantDBO);

                if (lResult)
                {
                    // success
                    TempData["msg"] = "<script>alert('Successfully Updated!')</script>";
                    return RedirectToAction("RestaurantProfile", "Restaurant", new { id = lRestaurantDBO.RestaurantIDPK });
                }
                else
                {
                    // failure
                    TempData["msg"] = "<script>alert('Error occured while processing your request. Please try later.')</script>";
                }
            }
            else
            {
                // model state not valid
                TempData["msg"] = "<script>alert('Please Fill all Required Fields.')</script>";
            }

            // Add days to the list
            iRestaurant.DayList = new List<string>();
            iRestaurant.DayList.Add("Monday");
            iRestaurant.DayList.Add("Tuesday");
            iRestaurant.DayList.Add("Wednesday");
            iRestaurant.DayList.Add("Thursday");
            iRestaurant.DayList.Add("Friday");
            iRestaurant.DayList.Add("Saturday");
            iRestaurant.DayList.Add("Sunday");

            return View(iRestaurant);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Admin")]
        public ActionResult RestaurantRemove(int id)
        {
            // Instantiate BLL object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // get result of bool if removal succeded
            bool lResult = lRestaurantBLL.RemoveRestaurantByRestaurantID(id);

            return RedirectToAction("RestaurantList", "Restaurant");
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult OrderCheck(int id)
        {
            // Restaurant BLL objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // get list of orders for the restaurant within a week
            List<UserOrderDBO> lUserOrderList = lRestaurantBLL.GetAllRecentOrdersByRestaurantID(id);

            if(lUserOrderList.Count == 0)
            {
                TempData["msg"] = "<script>alert('No orders');</script>";
                return RedirectToAction("Index", "Restaurant");
            }
            return PartialView("OrderCheck", lUserOrderList);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult FoodItemRemove(int id, int restaurantID)
        {
            //Instantiate BLL object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // get bool value if food item is successfully removed
            bool lResult = lRestaurantBLL.RemoveFoodItemByFoodItemID(id);

            if (lResult)
            {
                // success
                TempData["msg"] = "<script>alert('Successfully Updated!')</script>";
                return RedirectToAction("RestaurantProfile", "Restaurant", new { id = restaurantID, searchString = "reset" });
            }
            else
            {
                // failure
                TempData["msg"] = "<script>alert('Error occured while processing your request. Please try later.')</script>";
            }

            // set current tab
            return RedirectToAction("RestaurantProfile", "Restaurant", new { id = restaurantID, searchString = "reset" });
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult FoodItemCreate(int id)
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lRestaurantMapper = new RestaurantMapper();

            FoodItem lFoodItem = new FoodItem();

            // Get list of food types and ingredient types
            List<FoodTypeDBO> lFoodTypeDBOList = lRestaurantBLL.GetAllFoodTypes(); // main, side, etc
            List<IngredientTypeDBO> lIngredientTypeDBOList = lRestaurantBLL.GetAllFoodIngredients(); // for sandwich

            // Map List of DB objects to List of Model
            List<FoodType> lFoodTypeList = lRestaurantMapper.MapDBOToModel(lFoodTypeDBOList);
            List<IngredientType> lIngredientList = lRestaurantMapper.MapDBOToModel(lIngredientTypeDBOList);

            // set values
            lFoodItem.FoodTypeList = lFoodTypeList;
            lFoodItem.IngredientTypeList = lIngredientList;
            lFoodItem.RestaurantIDFK = id;
            lFoodItem.IsSandwichRestaurant = lRestaurantBLL.GetBoolSandwichByRestaurantID(id);

            return PartialView("FoodItemCreate", lFoodItem);
        }

        [HttpPost]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult FoodItemCreate(FoodItem iFoodItem)
        {
            if (ModelState.IsValid)
            {
                // Instantiates objects
                RestaurantBLL lRestaurantBLL = new RestaurantBLL();
                RestaurantMapper lRestaurantMapper = new RestaurantMapper();

                // map Model object to database object
                FoodItemDBO lFoodItemDBO = lRestaurantMapper.MapModelToDBO(iFoodItem);

                // get a unique food item id as return value
                int lFoodItemIDPK = lRestaurantBLL.CreateFoodItem(lFoodItemDBO);

                // success
                if (lFoodItemIDPK > 0)
                {
                    return Json(new { success = true, blank = "" });
                }
            }
            else
            {
                // check errors
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return Json(new { success = false });
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult FoodItemUpdate(int id)
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            RestaurantMapper lRestaurantMapper = new RestaurantMapper();

            // Find Food item by id
            FoodItemDBO lFoodItemDBO = lRestaurantBLL.FindFoodItemByFoodItemID(id);
            FoodItem lFoodItem = lRestaurantMapper.MapDBOToModel(lFoodItemDBO);

            // find the list of food types and lists
            List<FoodTypeDBO> lFoodTypeDBOList = lRestaurantBLL.GetAllFoodTypes();// main, side, etc
            List<IngredientTypeDBO> lIngredientTypeDBOList = lRestaurantBLL.GetAllFoodIngredients(); // for sandwich

            // Map List of DB Objects to List of Model
            List<FoodType> lFoodTypeList = lRestaurantMapper.MapDBOToModel(lFoodTypeDBOList);
            List<IngredientType> lIngredientList = lRestaurantMapper.MapDBOToModel(lIngredientTypeDBOList);

            lFoodItem.FoodTypeList = lFoodTypeList;
            lFoodItem.IngredientTypeList = lIngredientList;

            // find food info frmo db
            return PartialView("FoodItemUpdate", lFoodItem);
        }

        [HttpPost]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult FoodItemUpdate(FoodItem iFoodItem)
        {
            if (ModelState.IsValid)
            {
                // Instantiate objects
                RestaurantBLL lRestaurantBLL = new RestaurantBLL();
                RestaurantMapper lRestaurantMapper = new RestaurantMapper();

                // Map Model to DB obejcts
                FoodItemDBO lFoodItemDBO = lRestaurantMapper.MapModelToDBO(iFoodItem);

                // get result of bool if successfully updated
                bool lResult = lRestaurantBLL.UpdateFoodItemByFoodItemID(lFoodItemDBO);

                if (lResult)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult RestaurantPayment()
        {
            // Instantiate objects
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // Do calculations
            MeaningfulCalcUtil lCalcUtil = lRestaurantBLL.GetAllCalc();

            // return the weeks and show on the left menu bar
            return View(lCalcUtil);

        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult GetTopRatedOrder(int id)
        {
            // Instantiate Object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // get the list of ratings
            List<UserOrderRatingDBO> lUserOrderRating = lRestaurantBLL.GetTopRatedOrdersByRestaurantID(id);

            return PartialView(lUserOrderRating);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult SendOrders(int id)
        {
            // Instantiate Object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            bool isSent = lRestaurantBLL.sendOrdersByRestaurantID(id);

            return PartialView();
        }
    }
}