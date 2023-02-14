using Code_Challenge.DB;
using Code_Challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Data;
using System.Diagnostics;
using Code_Challenge.Models.UserModel;
using Windows.UI.Popups;

namespace Code_Challenge.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDBContext userDBContext;

        public UserController(UserDBContext userDBContext)
        {
            this.userDBContext = userDBContext;
        }


        //Index page

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await userDBContext.Users.ToListAsync();
            return View(users);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }


        [HttpGet]
        public FileResult ExportExcel()
        {
            string[] colNames = new string[] { "Id", "Username", "Email", "Password" };

            string csv = string.Empty;

            foreach(string colName in colNames)
            {
                csv += colName + ',';
            }

            csv += "\r\n";

            foreach (var users in userDBContext.Users.ToList())
            {
                csv += users.Id.ToString().Replace(",", ",") + ',';
                csv += users.Username.Replace(",", ",") + ',';
                csv += users.Email.Replace(",", ",") + ',';
                csv += users.Password.Replace(",", ",") + ',';

                csv += "\r\n";
            }

            byte[] bytes = Encoding.ASCII.GetBytes(csv);
            return File(bytes, "text/csv", "User.csv");
        }



        //Create new user Page

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }




        //Function to create a new user, new user is saved to the database

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUser)
        {
            var user = new User()
            {
                Id = createUser.Id,
                Username = createUser.Username,
                Email = createUser.Email,
                Password = createUser.Password
            };

            await userDBContext.Users.AddAsync(user);
            await userDBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        //Retrieve the user from the database on the Index page that will be edited

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var editUser = await userDBContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (editUser != null)
            {
                var editModel = new EditUserViewModel()
                {
                    Id = editUser.Id,
                    Username = editUser.Username,
                    Email = editUser.Email,
                    Password = editUser.Password
                };
                return View(editModel);
            }

            return RedirectToAction("Index");
        }


        // Edit a user, changes are saved to the Database

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel edit)
        {
            var user = await userDBContext.Users.FindAsync(edit.Id);

            if (user != null)
            {
                user.Username = edit.Username;
                user.Email = edit.Email;
                user.Password = edit.Password;

                await userDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        //Delete a user in the Edit page, deletes the user from the database

        [HttpPost]
        public async Task<IActionResult> Delete(EditUserViewModel delete)
        {
            var user = await userDBContext.Users.FindAsync(delete.Id);

            if (user != null)
            {
                userDBContext.Users.Remove(user);
                await userDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        //Login to access export list

        [HttpGet]
        public IActionResult Login()
        {
            Login login = new Login();
            return View(login);
        }


        [HttpPost]
        public IActionResult Login(Login login)
        {
            UserDBContext _userDBContext = new UserDBContext();
            var status = _userDBContext.Users.Where(m => m.Username == login.Username && m.Password == login.Password).FirstOrDefault();

            if(status == null)
            {
                ViewBag.LoginStatus = 0;
            }
            else
            {
                return RedirectToAction("Success");
            }

            return View(login);
        }

        public IActionResult Success()
        {
            return View("Export");
        }
    }
}