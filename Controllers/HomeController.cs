using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DonationPortal.Models;

namespace DonationPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        // ---------------------- Rendering login/reg page  ----------------------------------

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // ---------------------- process the registration form  ----------------------------------

        [HttpPost("register")]
        public IActionResult Register(User userToRegister)
        {
            // check for same/duplicate emails registered
            if (_context.Users.Any(u => u.Email == userToRegister.Email))
            {
                // add an error
                ModelState.AddModelError("Email", "Please use a different Email");
            }

            // check your validation errors
            if (ModelState.IsValid)
            {
                // hashing th password
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                userToRegister.Password = Hasher.HashPassword(userToRegister, userToRegister.Password);

                // insert the user into database
                _context.Add(userToRegister);
                // at this point user is added to the database with an ID and is saved
                _context.SaveChanges();

                // put the ID in the session
                HttpContext.Session.SetInt32("UserId", userToRegister.UserId);

                // redirect to the dashboard
                return RedirectToAction("Home");
            }

            // validations must have triggered
            return View("Index");
            // ^we are returning the rendered view above!!
        }

        // ---------------------------this is Login function ----------------------------------
        [HttpPost("login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            // look in your Database
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.LoginEmail);

            if (foundUser == null)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            // check if the passwords match??
            var hasher = new PasswordHasher<LoginUser>();

            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);

            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }

            //  set ID session
            HttpContext.Session.SetInt32("UserId", foundUser.UserId);
            return RedirectToAction("Home");
        }

        // -------------------------Logout Function  ---------------------------------
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // -------------------------Get current logged in User's information ----------------------------------
        public User GetCurrentUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return null;
            }

            return _context
                .Users
                .First(u => u.UserId == userId);
        }
        // -------------------------Home page  ----------------------------------

        [HttpGet("home")]
        public IActionResult Home()
        {

            var userInfo = GetCurrentUser();
            ViewBag.User = _context
            .Users
            .First(user => user.UserId == userInfo.UserId);

            ViewBag.Organizations = _context
                .Organizations
                .Include(i => i.Creator);

            return View();
        }

        // -------------------------Home page  ----------------------------------

        [HttpGet("viewPost/{orgId}")]
        public IActionResult Detail(int orgId)
        {

            ViewBag.Organizations = _context
                .Organizations
                .Include(o => o.Creator)
                .First(o => o.OrganizationId == orgId);

            var userInfo = GetCurrentUser();
            ViewBag.User = _context
            .Users
            .First(user => user.UserId == userInfo.UserId);

            return View();
        }

// -------------------------Clicks on Pledge  ---------------------------------
        [HttpGet("pledge/{orgId}")]
        public IActionResult Pledge(int orgId)
        {
            ViewBag.Organizations = _context
            .Organizations
            .First(o => o.OrganizationId == orgId);

            return View();
        }


        //---------------------------Posts A Donation ------------------------------------------------------------------
        [HttpPost("submitpledge/{orgid}")]

        public IActionResult AddPledge(int orgId)
        {

            if (ModelState.IsValid)
            {
                var org = _context
                .Organizations
                .First(o => o.OrganizationId == orgId);

                var input = Request.Form["Amount"];
                org.AmountCollected += Int32.Parse(input);

                _context.Update(org);
                _context.SaveChanges();

                User pledgeyear = _context
                .Users
                .First(u => u.UserId == GetCurrentUser().UserId);

                pledgeyear.PledgeYear = Int32.Parse(Request.Form["PledgeYear"]);

                _context.Update(pledgeyear);
                _context.SaveChanges();

                return RedirectToAction("Home");
            }
            return View("Home");
        }

        // -------------------------Add new Pledge  ---------------------------------
        [HttpGet("newPost")]
        public IActionResult AddPost()
        {
            return View();
        }

        //---------------------------Create  a new Post ------------------------------------------------------------------
        [HttpPost("addPost")]

        public IActionResult AddPledge(Organization dataFromForm)
        {

            if (ModelState.IsValid)
            {

                var user = GetCurrentUser();

                dataFromForm.Creator = user;

                _context.Add(dataFromForm);
                _context.SaveChanges();

                return RedirectToAction("Home");
            }
            return View("AddPost");
        }

        // -------------------------Add Donation  ---------------------------------
        [HttpGet("pledge")]
        public IActionResult Pledge()
        {
            return View();
        }

        // -------------------------Filter by zipcode  ---------------------------------
        [HttpPost("filterByZipcode")]
        public IActionResult Filter()
        {
            if (ModelState.IsValid)
            {
                var zipcode = Request.Form["ZipCode"];
                ViewBag.Organizations = _context
                .Organizations
                .Include(i => i.Creator)
                .First(o => o.ZipCode == Int32.Parse(zipcode));

                var userInfo = GetCurrentUser();
                ViewBag.User = _context
                .Users
                .First(user => user.UserId == userInfo.UserId);

                return View("Home");
            }
            return View("Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
