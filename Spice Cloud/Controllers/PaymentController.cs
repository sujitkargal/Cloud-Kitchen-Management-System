using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using Spice_Cloud.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;

namespace Spice_Cloud.Controllers
{
    using RazorpayOrder = Razorpay.Api.Order; // Razorpay alias
    using RazorpayClient = Razorpay.Api.RazorpayClient;

    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public PaymentController(IConfiguration configuration, AppDbContext db)
        {
            _configuration = configuration;
            _db = db;
        }

        // -----------------------------------------
        // GET: Payment Page
        // -----------------------------------------
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("UserLogin", "Home");

            var cartItems = _db.Carts
                .Include(c => c.Dish)
                .Where(c => c.UserId == userId.Value)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("ViewCart", "Home");

            // Calculate total price
            decimal totalAmount = cartItems.Sum(c =>
                (c.Dish.Price - (c.Dish.Price * c.Dish.Discount / 100)) * c.Quantity
            );

            // Check if a pending order already exists for this user and this cart amount
            var existingOrder = _db.Orders
                .Where(o => o.UserId == userId.Value && o.Status == "Pending")
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            RazorpayOrder razorOrder;
            string key = _configuration["Razorpay:Key"];
            string secret = _configuration["Razorpay:Secret"];
            RazorpayClient client = new RazorpayClient(key, secret);

            if (existingOrder != null)
            {
                // Use existing order
                razorOrder = new RazorpayOrder { }; // Placeholder, we only need OrderId
            }
            else
            {
                int amountInPaise = (int)(totalAmount * 100); // Razorpay needs paise

                Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amountInPaise },
            { "currency", "INR" },
            { "payment_capture", 1 }
        };

                razorOrder = client.Order.Create(options);

                // Save new pending order
                var order = new Spice_Cloud.Models.Order
                {
                    UserId = userId.Value,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    RazorpayOrderId = razorOrder["id"].ToString(),
                    PaymentId = "",
                    OrderDate = DateTime.Now
                };

                try
                {
                    _db.Orders.Add(order);
                    _db.SaveChanges();
                    existingOrder = order;
                }
                catch (Exception ex)
                {
                    return Content("ERROR: " + ex.Message + " --- " + ex.InnerException?.Message);
                }
            }

            // Use existing order's RazorpayOrderId
            var model = new PaymentViewModel()
            {
                Key = key,
                Amount = (int)(totalAmount * 100),
                Currency = "INR",
                OrderId = existingOrder.RazorpayOrderId
            };

            return View(model);
        }


        // -----------------------------------------
        // POST: Payment Success Callback
        // -----------------------------------------
        [HttpPost]
        public IActionResult Success(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            string secret = _configuration["Razorpay:Secret"];
            string payload = $"{razorpay_order_id}|{razorpay_payment_id}";

            // Verify signature
            string generatedSignature = GenerateSHA256Hash(payload, secret);
            bool isValid = generatedSignature == razorpay_signature;

            if (!isValid)
            {
                ViewBag.Message = "Payment verification failed!";
                return View("Failed");
            }

            // Fetch the order created before payment
            var order = _db.Orders.FirstOrDefault(o => o.RazorpayOrderId == razorpay_order_id);

            if (order == null)
            {
                ViewBag.Message = "Order not found!";
                return View("Failed");
            }

            // Update Order Status
            order.PaymentId = razorpay_payment_id;
            order.Status = "Paid";
            _db.SaveChanges();

            // Save Order Items
            int? userId = HttpContext.Session.GetInt32("UserId");

            var cartItems = _db.Carts
                .Include(c => c.Dish)
                .Where(c => c.UserId == userId.Value)
                .ToList();

            foreach (var item in cartItems)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    Price = item.Dish.Price,
                    Discount = item.Dish.Discount,
                    FinalPrice = (item.Dish.Price - (item.Dish.Price * item.Dish.Discount / 100)) * item.Quantity
                });
            }

            // Clear cart
            _db.Carts.RemoveRange(cartItems);
            _db.SaveChanges();

            // Return data to success page
            ViewBag.PaymentId = razorpay_payment_id;
            ViewBag.OrderId = razorpay_order_id;
            ViewBag.Amount = order.TotalAmount;

            return View();
        }

        // -----------------------------------------
        // Hash Generator
        // -----------------------------------------
        private string GenerateSHA256Hash(string input, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
