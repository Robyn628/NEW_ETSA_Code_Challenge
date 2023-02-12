using Code_Challenge.DB;
using Code_Challenge.Models;
using Code_Challenge.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        //Create new user Page

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        //Function to create a new user, new user is saved to the database

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel addUser)
        {
            var user = new User()
            {
                Id = addUser.Id,
                Username = addUser.Username,
                Email = addUser.Email,
                Password = addUser.Password
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

            if(user != null)
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
    }
}
