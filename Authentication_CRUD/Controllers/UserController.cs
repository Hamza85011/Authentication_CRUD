using CRUD_ADO.NET.Services;
using Authentication_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Http;

public class UserController : Controller
{
    private readonly UserDAL _userDAL;
    public object FormsAuthentication { get; private set; }
    public UserController(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("cs");
        _userDAL = new UserDAL(connectionString);
    }
    [HttpGet]
    public ActionResult CreateAccount()
    {
        return View();
    }
    [HttpPost]
    public ActionResult CreateAccount(UserLogin login)
    {
        if (ModelState.IsValid)
        {
            if (_userDAL.CreateAccount(login))
            {
                TempData["Message"] = "Account Created Successfully";
                return RedirectToAction("SignIn");
            }
            else
            {
                TempData["InsertMsge"] = "Username and password already exists";
                return RedirectToAction("CreateAccount");
            }
        }
        return View(login);
    }
    [HttpGet]
    public ActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public ActionResult SignIn(UserLogin login)
    {
        bool isValidUser = _userDAL.SignIn(login);
        if (isValidUser)
        {
            HttpContext.Session.SetString("Name", login.Username);
            HttpContext.Session.SetString("Password", login.Password);
            return RedirectToAction("GetList");
        }
        else
        {
            TempData["InsertMsge"] = "Invalid Username and Password";
            return RedirectToAction("SignIn");
        }
    }
    public IActionResult GetList()
    {
        List<UserModelcs> users = _userDAL.GetList();
        return View(users);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(UserModelcs model)
    {
        if (ModelState.IsValid)
        {
            if (_userDAL.Create(model))
            {
                TempData["Message"] = "User data added successfully.";
                return RedirectToAction("GetList");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to save user data.");

            }
        }
        return View(model);
    }
    [HttpGet]
    public IActionResult Update(int id)
    {
        UserModelcs model = _userDAL.GetDetails(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
    [HttpPost]
    public IActionResult Update(UserModelcs model)
    {
        if (ModelState.IsValid)
        {
            if (_userDAL.Update(model))
            {
                TempData["InsertMsge"] = "User update your data successfully.";
                return RedirectToAction("GetList");
            }
            else
            {
                TempData["InsertMsg"] = "Failed to save user data.";
            }
        }
        return View(model);
    }
    public IActionResult Details(int id)
    {
        UserModelcs model = _userDAL.GetDetails(id);
        return View(model);
    }
    public IActionResult Delete(int id)
    {
        bool isDeleted = _userDAL.Delete(id);
        if (isDeleted)
        {
            TempData["DelectMsg"] = "User Delete your data successfully.";
        }
        else
        {
            TempData["DeleteMsg"] = "Failed to delete user data.";
        }
        return RedirectToAction("GetList");
    }
}
