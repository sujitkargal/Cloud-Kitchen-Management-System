using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spice_Cloud.Models;

namespace Spice_Cloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        // ------------------- CONSTRUCTOR -------------------
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        // ------------------- DEFAULT PAGES -------------------
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        // ------------------- CHEF REGISTER -------------------
        public IActionResult ChefRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChefRegister(Chef chef)
        {
            if (!ModelState.IsValid)
                return View(chef);

            var existChef = _db.Chefs.FirstOrDefault(c => c.Email == chef.Email);
            if (existChef != null)
            {
                TempData["MessageC"] = "Chef already registered. Please login.";
                return RedirectToAction("ChefLogin");
            }

            if (chef.Password != chef.ConfirmPassword)
            {
                TempData["MessageC"] = "Password and Confirm Password do not match.";
                return View(chef);
            }

            _db.Chefs.Add(chef);
            _db.SaveChanges();

            TempData["MessageC"] = "Registered successfully! Please login.";
            return RedirectToAction("ChefLogin");
        }

        // ------------------- CHEF LOGIN -------------------
        public IActionResult ChefLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChefLogin(string email, string password)
        {
            var chef = _db.Chefs.FirstOrDefault(c => c.Email == email);

            if (chef == null)
            {
                TempData["MessageC"] = "Email not found. Please register first.";
                return RedirectToAction("ChefLogin");
            }

            if (chef.Password != password)
            {
                TempData["MessageC"] = "Incorrect password. Please try again.";
                return RedirectToAction("ChefLogin");
            }

            HttpContext.Session.SetString("ChefName", chef.Name);
            HttpContext.Session.SetInt32("ChefId", chef.ChefId);
            TempData["ChefName"] = chef.Name;

            return RedirectToAction("ChefDashboard");
        }

        // ------------------- FORGOT PASSWORD -------------------
        public IActionResult ForgotChefPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotChefPassword(string email)
        {
            //var chef = _db.Chefs.FirstOrDefault(c => c.Email == email);
            //if (chef == null)
            //{
            //    TempData["MessageC"] = "Email not found. Please register first.";
            //    return RedirectToAction("ChefRegister");
            //}

            //// Logic to send password reset email or show reset form
            //TempData["MessageC"] = "Check your email for password reset instructions.";
            return RedirectToAction("ChefLogin");
        }

        // ------------------- CHEF DASHBOARD -------------------
        public IActionResult ChefDashboard()
        {
            var chefName = HttpContext.Session.GetString("ChefName");
            if (string.IsNullOrEmpty(chefName))
            {
                TempData["MessageC"] = "Please login first.";
                return RedirectToAction("ChefLogin");
            }

            ViewBag.ChefName = chefName;
            return View();
        }

        // ------------------- LOGOUT -------------------
        public IActionResult ChefLogout()
        {
            HttpContext.Session.Clear();
            TempData["MessageC"] = "Logged out successfully.";
            return RedirectToAction("Index");
        }

        // ------------------- USER REGISTER -----------------------------------------------------------
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegister(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existUser = _db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existUser != null)
            {
                TempData["MessageU"] = "User already registered. Please login.";
                return RedirectToAction("UserLogin");
            }

            if (user.Password != user.ConfirmPassword)
            {
                TempData["MessageU"] = "Password and Confirm Password do not match.";
                return View(user);
            }

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["MessageU"] = "Registered successfully! Please login.";
            return RedirectToAction("UserLogin");
        }

        // ------------------- USER LOGIN -------------------
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(string email, string password)
        {
       
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["MessageU"] = "Email not found. Please register first.";
                return RedirectToAction("UserLogin");
            }

            if (user.Password != password)
            {
                TempData["MessageU"] = "Incorrect password. Please try again.";
                return RedirectToAction("UserLogin");
            }

            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetInt32("UserId", user.Id);

            TempData["UserName"] = user.Name;

            return RedirectToAction("UserDashboard");
        }


        // ------------------- USER DASHBOARD -------------------
        public IActionResult UserDashboard()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                TempData["MessageU"] = "Please login first.";
                return RedirectToAction("UserLogin");
            }

            ViewBag.UserName = userName;
            return View();
        }


        // ------------------- USER LOGOUT -------------------
        public IActionResult UserLogout()
        {
            HttpContext.Session.Clear();
            TempData["MessageU"] = "Logged out successfully.";
            return RedirectToAction("Index");
        }

        // ------------------- USER FORGOT PASSWORD -------------------
        public IActionResult ForgotUserPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotUserPassword(string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["MessageU"] = "Email not found. Please register first.";
                return RedirectToAction("UserRegister");
            }

            TempData["MessageU"] = "Check your email for password reset instructions.";
            return RedirectToAction("UserLogin");
        }


        // ------------------- ADMIN REGISTER -------------------
        public IActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminRegister(Admin admin)
        {
            if (!ModelState.IsValid)
                return View(admin);

            var existAdmin = _db.Admins.FirstOrDefault(a => a.Email == admin.Email);
            if (existAdmin != null)
            {
                TempData["MessageA"] = "Admin already registered. Please login.";
                return RedirectToAction("AdminLogin");
            }

            if (admin.Password != admin.ConfirmPassword)
            {
                TempData["MessageA"] = "Password and Confirm Password do not match.";
                return View(admin);
            }

            _db.Admins.Add(admin);
            _db.SaveChanges();

            TempData["MessageA"] = "Admin registered successfully!";
            return RedirectToAction("AdminLogin");
        }

        // ------------------- ADMIN LOGIN -------------------
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string email, string password)
        {
            
            var admin = _db.Admins.FirstOrDefault(a => a.Email == email);

            if (admin == null)
            {
                TempData["MessageA"] = "Email not found. Please register first.";
                return RedirectToAction("AdminLogin");
            }

            if (admin.Password != password)
            {
                TempData["MessageA"] = "Incorrect password. Please try again.";
                return RedirectToAction("AdminLogin");
            }
            HttpContext.Session.SetString("AdminName", admin.Name);
            HttpContext.Session.SetInt32("AdminId", admin.AdminId);

            return RedirectToAction("AdminDashboard");
        }


        // ------------------- ADMIN DASHBOARD -------------------
        public IActionResult AdminDashboard()
        {
            var adminName = HttpContext.Session.GetString("AdminName");

            if (string.IsNullOrEmpty(adminName))
            {
                TempData["MessageA"] = "Please login first.";
                return RedirectToAction("AdminLogin");
            }

            ViewBag.AdminName = adminName;
            return View();
        }
        // ------------------- ADMIN MANAGEMENT -------------------

        public IActionResult ManageUsers()
        {
            var users = _db.Users.ToList();
            return View(users);
        }

        public IActionResult ManageChefs()
        {
            var chefs = _db.Chefs.ToList();
            return View(chefs);
        }


        // ------------------- EDIT USER -------------------

        public IActionResult EditUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _db.Users.Update(user);
            _db.SaveChanges();

            TempData["MessageA"] = "User updated successfully!";
            return RedirectToAction("ManageUsers");
        }


        // ------------------- DELETE USER -------------------

        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            _db.SaveChanges();

            TempData["MessageA"] = "User deleted!";
            return RedirectToAction("ManageUsers");
        }


        // ------------------- EDIT CHEF -------------------

        public IActionResult EditChef(int id)
        {
            var chef = _db.Chefs.Find(id);
            if (chef == null)
                return NotFound();

            return View(chef);
        }

        [HttpPost]
        public IActionResult EditChef(Chef chef)
        {
            if (!ModelState.IsValid)
                return View(chef);

            _db.Chefs.Update(chef);
            _db.SaveChanges();

            TempData["MessageA"] = "Chef updated successfully!";
            return RedirectToAction("ManageChefs");
        }


        // ------------------- DELETE CHEF -------------------

        public IActionResult DeleteChef(int id)
        {
            var chef = _db.Chefs.Find(id);
            if (chef == null)
                return NotFound();

            _db.Chefs.Remove(chef);
            _db.SaveChanges();

            TempData["MessageA"] = "Chef deleted!";
            return RedirectToAction("ManageChefs");
        }

        // ------------------- ADMIN LOGOUT -------------------
        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            TempData["MessageA"] = "Logged out successfully.";
            return RedirectToAction("Index");
        }

        // ------------------- ADMIN FORGOT PASSWORD -------------------
        public IActionResult ForgotAdminPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotAdminPassword(string email)
        {
            var admin = _db.Admins.FirstOrDefault(a => a.Email == email);
            if (admin == null)
            {
                TempData["MessageA"] = "Email not found. Please register first.";
                return RedirectToAction("AdminRegister");
            }

            TempData["MessageA"] = "Check your email for password reset instructions.";
            return RedirectToAction("AdminLogin");
        }

        // ---------------------- ADD DISH ----------------------
        public IActionResult AddDish()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDish(Dish dish)
        {
            if (dish.ImageFile != null && dish.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/dishes");
                Directory.CreateDirectory(uploadDir);

                string extension = Path.GetExtension(dish.ImageFile.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(uploadDir, fileName);

           
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dish.ImageFile.CopyToAsync(stream);
                }

                dish.ImagePath = "/images/dishes/" + fileName;
            }

            int? chefId = HttpContext.Session.GetInt32("ChefId");
            if (chefId.HasValue)
                dish.ChefId = chefId.Value;

            _db.Dishes.Add(dish);
            await _db.SaveChangesAsync();

            return RedirectToAction("ChefDashboard");
        }


       //-------------- show chef dish -------------
        public async Task<IActionResult> ChefDish()
        {
            int? chefId = HttpContext.Session.GetInt32("ChefId");
            if (!chefId.HasValue)
            {
                TempData["MessageC"] = "Please login first.";
                return RedirectToAction("ChefLogin");
            }

            var dishes = await _db.Dishes
                .Where(d => d.ChefId == chefId.Value) 
                .ToListAsync();

            return View(dishes);
        }


        // ----------------- Show edit form --------------
        public async Task<IActionResult> UpdateDish(int id)
        {
            var dish = await _db.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }
        //---------------------show dish-------------------
        public async Task<IActionResult> ChefDishes()
        {
            int? chefId = HttpContext.Session.GetInt32("ChefId");
            if (!chefId.HasValue)
            {
                return RedirectToAction("ChefLogin", "Home"); // or your login page
            }

            var dishes = await _db.Dishes
                .Where(d => d.ChefId == chefId.Value)
                .ToListAsync();

            return View(dishes);
        }



        //--------- Update Dish ---------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDish(Dish dish)
        {
            if (!ModelState.IsValid)
            {
                return View(dish);
            }

            var existingDish = await _db.Dishes.FindAsync(dish.Id);
            if (existingDish == null)
            {
                return NotFound();
            }

            existingDish.DishName = dish.DishName;
            existingDish.Category = dish.Category;
            existingDish.Price = dish.Price;
            existingDish.Discount = dish.Discount;
            existingDish.Description = dish.Description;

            if (dish.ImageFile != null && dish.ImageFile.Length > 0)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/dishes");
                Directory.CreateDirectory(uploadDir);

                string extension = Path.GetExtension(dish.ImageFile.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dish.ImageFile.CopyToAsync(stream);
                }

                existingDish.ImagePath = "/images/dishes/" + fileName;
            }

            _db.Dishes.Update(existingDish);
            await _db.SaveChangesAsync();

            return RedirectToAction("ChefDashboard");
        }
        //------------------------Delet Dish---------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _db.Dishes.FindAsync(id);
            int? chefId = HttpContext.Session.GetInt32("ChefId");

            if (dish == null || chefId == null || dish.ChefId != chefId.Value)
            {
                return NotFound();
            }

            _db.Dishes.Remove(dish);
            await _db.SaveChangesAsync();

            return RedirectToAction("ChefDishes");
        }


        // ------------------- Load Dishes -------------------
        [HttpGet]
        public IActionResult LoadDishes(string search = "", string category = "All", int page = 1, int pageSize = 6)
        {
            var dishes = _db.Dishes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                dishes = dishes.Where(d => d.DishName.Contains(search));

            if (!string.IsNullOrEmpty(category) && category != "All")
                dishes = dishes.Where(d => d.Category == category);

            var pagedDishes = dishes
                .OrderBy(d => d.DishName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    d.Id,
                    d.DishName,
                    d.Description,
                    d.ImagePath,
                    d.Price,
                    d.Discount,
                    d.Category
                })
                .ToList();

            return Json(pagedDishes);
        }

        [HttpPost]
        public IActionResult AddToCart(int DishId, int Quantity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new
                {
                    success = false,
                    loginRequired = true,
                    message = "Please login first to add items to your cart."
                });
            }

          
            var existingCart = _db.Carts.FirstOrDefault(c => c.UserId == userId && c.DishId == DishId);

            if (existingCart != null)
            {
                existingCart.Quantity += Quantity;
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = userId.Value,
                    DishId = DishId,
                    Quantity = Quantity
                };

                _db.Carts.Add(cartItem);
            }

            _db.SaveChanges();

            int cartCount = _db.Carts.Where(c => c.UserId == userId).Sum(c => c.Quantity);

            return Json(new
            {
                success = true,
                message = "Item added to cart!",
                cartCount = cartCount
            });
        }

        public void LoadCartCount()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                ViewBag.CartCount = _db.Carts.Where(c => c.UserId == userId).Sum(c => c.Quantity);
            }
            else
            {
                ViewBag.CartCount = 0;
            }
        }

        //-------------- Show Carts-----------
        public IActionResult ViewCart()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
                return RedirectToAction("UserLogin");

            var cartItems = _db.Carts
                .Where(c => c.UserId == userId.Value)
                .Include(c => c.Dish)
                .Select(c => new CartViewModel
                {
                    CartId = c.Id,
                    DishId = c.DishId,
                    DishName = c.Dish.DishName,
                    ImagePath = c.Dish.ImagePath,
                    Price = c.Dish.Price,
                    Discount = c.Dish.Discount,
                    Quantity = c.Quantity,
                    FinalPrice = c.Dish.Price - ((c.Dish.Price * c.Dish.Discount) / 100)
                })
                .ToList();

            return View(cartItems);
        }

        //-------------------------- remove Cart ------------------
        [HttpPost]
        public IActionResult RemoveFromCart(int cartId)
        {
            var item = _db.Carts.FirstOrDefault(c => c.Id == cartId);

            if (item != null)
            {
                _db.Carts.Remove(item);
                _db.SaveChanges();
            }

            return RedirectToAction("ViewCart");
        }


        //---------------------Order -----------------------------
        [HttpPost]
        public IActionResult OrderNow()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
                return RedirectToAction("UserLogin");

            var cartItems = _db.Carts
                .Where(c => c.UserId == userId.Value)
                .Include(c => c.Dish)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty!";
                return RedirectToAction("ViewCart");
            }

            decimal totalAmount = cartItems.Sum(c =>
                (c.Dish.Price - (c.Dish.Price * c.Dish.Discount / 100)) * c.Quantity
            );

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

           
            foreach (var item in cartItems)
            {
                var finalPrice = (item.Dish.Price - (item.Dish.Price * item.Dish.Discount / 100)) * item.Quantity;

                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    Price = item.Dish.Price,
                    Discount = item.Dish.Discount,
                    FinalPrice = finalPrice
                });
            }

            _db.Carts.RemoveRange(cartItems);
            _db.SaveChanges();
            return RedirectToAction("Index", "Payment", new { orderId = order.Id });
        }


        // ------------------------UserOrders--------------------------
        public IActionResult UserOrders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                TempData["MessageU"] = "Please login first!";
                return RedirectToAction("UserLogin", "Home");
            }

          
            var orders = _db.Orders
                            .Include(o => o.Items)
                                .ThenInclude(oi => oi.Dish)
                            .Where(o => o.UserId == userId.Value)
                            .OrderByDescending(o => o.OrderDate)
                            .ToList();

            return View(orders);
        }


        [HttpGet]
        public IActionResult CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return BadRequest("Invalid order ID.");

          
            var order = _db.Orders.FirstOrDefault(o => o.RazorpayOrderId == orderId && o.Status == "Pending");

            if (order == null)
                return NotFound("Order not found or already processed.");


            var orderItems = _db.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();
            _db.OrderItems.RemoveRange(orderItems);
            _db.Orders.Remove(order);
            _db.SaveChanges();

            TempData["Message"] = "Order cancelled successfully!";
            return RedirectToAction("UserOrders", "Home"); // Adjust to your order list page
        }


        public IActionResult ChefOrders()
        {
            int? chefId = HttpContext.Session.GetInt32("ChefId");
            if (!chefId.HasValue)
            {
                TempData["MessageC"] = "Please login first!";
                return RedirectToAction("ChefLogin");
            }

            // Get all OrderItems where the dish belongs to this chef
            var chefOrderItems = _db.OrderItems
                .Include(oi => oi.Dish)
                .Include(oi => oi.Order)
                .Where(oi => oi.Dish.ChefId == chefId.Value)
                .OrderByDescending(oi => oi.Order.OrderDate)
                .ToList();

         
            var ordersGrouped = chefOrderItems
                .GroupBy(oi => oi.Order)
                .Select(g => new ChefOrderViewModel
                {
                    OrderId = g.Key.Id,
                    RazorpayOrderId = g.Key.RazorpayOrderId,
                    OrderDate = g.Key.OrderDate,
                    Status = g.Key.Status,
                    TotalAmount = g.Key.TotalAmount,
                    Items = g.ToList()
                })
                .ToList();

            return View(ordersGrouped);
        }



        // ------------------- ERROR -------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
