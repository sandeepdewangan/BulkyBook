# ASP.NET Core 
Project of Udemy Course [Complete guide to ASP.NET Core MVC (v3.1)](https://www.udemy.com/course/complete-aspnet-core-21-course)

## Install Extension
1. Markdown Editor - README.md file editor.

## Install 3rd Party Tools

1. [jQuery Date Picker](https://jqueryui.com/datepicker/)
2. [Data Tables](https://datatables.net/)
3. [Sweet Alerts](https://sweetalert.js.org/)
4. [Toastr for toast message](https://github.com/CodeSeven/toastr)
5. [Font Awesome Icons](https://fontawesome.com/)

> All CSS and JS File Combined > Paste it to _Layout.cshtml file
```html
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.css">
<link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css">
<link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css">

<script src="https://kit.fontawesome.com/363284e9b5.js" crossorigin="anonymous"></script>
<script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.js"></script>
<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
```


## Install Packages

1. Runtime Compilation

Add in Startup.cs at ConfigureServices method
```c#
services.AddControllersWithViews().AddRazorRuntimeCompilation();
```


## Starting Project

1. Start Prject and Select -> ASP.NET Core Web MVC Application with Authentication as Individual User selected.

#### Project Refractoring

1. Add new project -> class library -> BulkyBook.DataAccess, BulkyBook.Models, BulkyBook.Utility (Core Class Library)
2. Install Packages to BulkyBook.DataAccess project
   i. Microsoft.EntityFrameworkCore.Relational
   2. Microsoft.EntityFrameworkCore.SqlServer
   3. Microsoft.EntityFrameworkCore.Identity.EntityFrameworkCore
3. Separating Data Layer
	1. Move the data folder from BulkyBook to BulkyBook.DataAccess.
    2. Delete the BulkyBook data folder.
    3. Delete the content of Migration folder from BulkyBook.DataAccess.
    4. Change Namespace of ApplicationDbContext.cs file.
4. Copy the ErrorViewModel file from Models folder and Paste it in BulkyBook.Models.ViewModel. After that delete Model folder of BulkyBook project.
5. BulkyBook.Utility class can be used for accessing the static fields. So the class is static.


#### Project Reference

1. Add following reference to BulkyBook project - DataAccess, Models and Utility.
2. Add following reference to BulkyBook.DataAccess project - Models and Utility.

## Areas

```bash
|-Areas
|--- Identity (Razor Class Library)
|--- Customer (MVC)
|------ Views
|------ Controller
|--- Admin (MVC)
```

#### Adding Customer Area

1. Add area, named Customer
2. Create folder structure as below

```bash
|-Areas
|--- Customer
|------ Views
|--------- Home
|------------ index.cshtml
|------ Controller
|--------- HomeController.cs
```

3. Explicitly Mark Area with annotations

```c#
[Area("Customer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
```

4. Temporary change startup.cs

```c#
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
```

5. Copy _ViewStart.cshtml and _ViewImports.cshtml to the Views folder of Customer area and add reference to the _Layout file.

```c#
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
```

## Bootstrap

1. Go to https://bootswatch.com/
2. Select one of the theme and copy the css.
3. Paste the css into wwwroot of project.
4. Goto the _Layout page and change the css file to use bootstrap.css
5. Make some changes to code in HTML to use the downloaded theme.


## Database Setup

1. Under appsettings.json, find connection string

```c#
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BulkyBook;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
```

> Note: Run migration commands from BulkyBook.DataAccess

2. Add migration - $ add-migration NameMigration
3. Update database - $ update-database
4. Create a Model class

```c#
public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name="Category Name")]
        public string Name { get; set; }
    }
```

5. Add Category to DbContext

```c#
public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
```

6. Now migrate and update database

## Repository Pattern

Under DataAccess project make directory structure as below.

```bash
|-BulkyBook.DataAccess
|--- Repository
|------ CategoryRepository.cs
|------ IRepository
|--------- ICategoryRepository.cs
```

```c#
namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
    }
}
```

```c#
public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _db.Categories.ToList();
            return categories;
        }
    }
```

> Add code snnipets to startup.cs

```c#
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<ICategoryRepository, CategoryRepository>(); // <---- THIS
```

> Usage of Repository (Dependency Injection)

```c#
[Area("Customer")]
public class HomeController : Controller
{
    private readonly ICategoryRepository repoCategory
    public HomeController(ICategoryRepository repoCategory)
    {
        this.repoCategory = repoCategory;
    }
    public IActionResult Index()
    {
        var categories = repoCategory.GetAll();
        return View(categories);
    }
}
```


## Comments

> Inside C# File

```c#
#region API CALLS
#endregion
```

> Inside cshtml

```c#
@* Comment Goes HERE *@
```

## Database CRUD

File Structure

```bash
|-Areas
|--- Admin
|------ Controllers
|--------- CategoryController
|------ Views
|--------- Category
|------------ Index.cshtml.cs
```

```bash
|-wwwroot
|--- js
|----- category.js
```

**See Commit: Category CRUD**

## User Registration

1. Create a class which will override the existing model which was created by VS throught Identity.

```c#
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public int? CompanyId { get; set; }
        [NotMapped] // Not Added to the database
        public string Role { get; set; } 

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}
```

2. Add class to Application DB Context

```c#
public DbSet<ApplicationUser> ApplicationUsers { get; set; }
```

3. Migrate and Update database.

#### Adding these fields to Register Page

1. Right click on project and select new scafolded item.
2. Select identity > add.
3. Select layout.
3. Select all pages and select applciation db context.
5. Under Register.cshtml.cs > on InputModel class paste our custom model.

> Input Model is used for Registration.
```c#
public class InputModel
 {
  ......

     [Required]
     public string Name { get; set; }
     public string StreetAddress { get; set; }
     public string City { get; set; }
     public string State { get; set; }
     public string PostalCode { get; set; }
     public int? CompanyId { get; set; }
     public string Role { get; set; }

// Foreign key mappings are not needed here because none of these properties will be mapped to db.
 }
```

6. Add to Register.cshtml (add all name, streetaddress, city etc)

```html
<div class="form-group">
   <label asp-for="Input.Name"></label>
   <input asp-for="Input.Name" class="form-control" />
   <span asp-validation-for="Input.Name" class="text-danger"></span>
</div>
```

7. Change method OnPostAsync()

**Remove**
```c#
var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
```
**Add**
```c#
public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
  if (ModelState.IsValid)
  {
      //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
      var user = new ApplicationUser
      {
          UserName = Input.Name,
          Email = Input.Email,
          CompanyId = Input.CompanyId,
          StreetAddress = Input.StreetAddress,
          City = Input.City,
          State = Input.State,
          PostalCode = Input.PostalCode,
          Name = Input.Name,
          Role = Input.Role
      };
}
```
> Comment the email confirmation mail.

```c#
 //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
 //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
 //var callbackUrl = Url.Page(
 //    "/Account/ConfirmEmail",
 //    pageHandler: null,
 //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
 //    protocol: Request.Scheme);

 //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
 //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
```
8. Inject Role Manager and Our Database Accessing Class (eg, Unit of Work)

```c#
public class RegisterModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RegisterModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
}

```

9. Create Role Dynamically and Assigning User Some Role
```c#
Register.cs

if (result.Succeeded)
{
    _logger.LogInformation("User created a new account with password.");

    // Role Creation Dynamically

    if(!await _roleManager.RoleExistsAsync(SD.Role_Admin))
    {
        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
    }
    if (!await _roleManager.RoleExistsAsync(SD.Role_Employee))
    {
        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
    }
    if (!await _roleManager.RoleExistsAsync(SD.Role_User))
    {
        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
    }

    // Assigning User to Some Role
    await _userManager.AddToRoleAsync(user, SD.Role_Admin);
}
```

10. Change Startup.cs file to configure Roles

> The AddDefaultIdentity doesnot support Roles.
> We have commented out the code for sending email, in that we have code for generating token. To save our application from crashing, add DefaultTokenProviders at startup.
```c#
services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
```

11. Error for Email Providers - Microsoft.AspNetCore.Identity.UI.Services.IEmailSender

**Step 01**: Add a class to Utility Project
```c#
namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
```
**Step 02**: Configure startup.cs

```c#
services.AddSingleton<IEmailSender, EmailSender>();
```

## Registration - Dropdown Selection

1. Edit Register.cs InputModel
```c#
 public IEnumerable<SelectListItem> CompanyList { get; set; }
 public IEnumerable<SelectListItem> RoleList { get; set; }
```

2. Add Fields
```c#
 public async Task OnGetAsync(string returnUrl = null)
 {
     ReturnUrl = returnUrl;

     // Including dropdown box in register page
     Input = new InputModel()
     {
         CompanyList = _repoCoverType.GetAll().Select(i => new SelectListItem
         {
             Text = i.Name,
             Value = i.Id.ToString()
         }),

// this role is only visible to ADMIN.
         RoleList = _roleManager.Roles.Where(u => u.Name != SD.Role_User).Select(x => x.Name).Select(i => new SelectListItem
         {
             Text = i,
             Value = i
         })
     };
}
```

3. Edit cshtml page (only admin can see this)
```c#
@if (User.IsInRole(SD.Role_Admin))
   {
       <div class="form-group">
           <label asp-for="Input.Role"></label>
           @Html.DropDownListFor(m => m.Input.Role, Model.Input.RoleList, "-Please select a role", new { @class = "form-control" })
       </div>
       <div class="form-group">
           <label asp-for="Input.CompanyId"></label>
           @Html.DropDownListFor(m => m.Input.CompanyId, Model.Input.CompanyList, "-Please select company", new { @class = "form-control" })
       </div>
   }
```

4. Edit Register.cs file for registration of users based on roles.

**Remove**
```c#
// Assigning User to Some Role
await _userManager.AddToRoleAsync(user, SD.Role_Admin);
```

**Add**
```c#
if(user.Role == null) // if normal user register, it role will be null
 {
     await _userManager.AddToRoleAsync(user, SD.Role_User);
 }
 else
 {
     // Assign the role of company
     if(user.CompanyId > 0)
     {
         await _userManager.AddToRoleAsync(user, SD.Role_Employee);
     }

     // From one of the selected role
     await _userManager.AddToRoleAsync(user, user.Role);
 }

```
...
...

```c#
if(user.Role == null)
  {
      await _signInManager.SignInAsync(user, isPersistent: false);
      return LocalRedirect(returnUrl);
  }
  else
  {
      // admin is registering a new user, so we need to redirect to admin area
      return RedirectToAction("Index", "User", new { Area = "Admin" });
  }
```

## User Class
1. Create controller - UserController
2. Create
```c#
[HttpGet]
public IActionResult GetAll()
```

> SEE Commit - User Class

## Authorization

1. Add at startup page under Configure method.

```c#
app.UseAuthentication();
app.UseAuthorization();
```

2. Add annotation to controller
```c#
[Authorize(Roles = "Admin")] //<-- Use it from SD class (static)
[Authorize(Roles = Utility.SD.Role_Admin)]

or 

[Authorize(Roles = "Admin, Employee")]
[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
```

3. Add for page access denied (add to startup.cs)
```c#
public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
        }

```

## Display link based on Roles

```c#
@if(User.IsInRole("Admin")){
    //HTMl Links
}

or

@if(User.IsInRole(SD.Role_Admin)){
    //HTMl Links
}

```

## Deployment
**Step 01 Creating Admin User and Roles (Initial Setup)** 

```c#
namespace BulkyBook.DataAccess.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
```

**Step 02 Implement the DbInitializer Class** 
 ```c#
namespace BulkyBook.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                // Migrate pending migrations
                if(_dbContext.Database.GetPendingMigrations().Count () > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            // Everytime the application starts it will keep on creating the roles. so to avoid.
            if (_dbContext.Roles.Any(r => r.Name == SD.Role_Admin)) return;

            // Creating roles on the go, awaiter is used to stop the code here until it get results.
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();

            // Creating user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "sandeep@gmail.com",
                Email = "sandeep@gmail.com",
                EmailConfirmed = true,
                Name = "Sandeep Dewangan"
            }, "Sandeep123@").GetAwaiter().GetResult();

            // Get Admin User
            ApplicationUser user = _dbContext.ApplicationUsers.Where(u => u.Email == "sandeep@gmail.com").FirstOrDefault();

            // Assign the user to role of ADMIN
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
```
**Step 03 Inject Dependency (Startup.cs)**
```c#
services.AddScoped<IDbInitializer, DbInitializer>();
```

**Step 04 Configure DbInitializer**

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
{
    ....
    app.UseAuthentication();
    app.UseAuthorization();
    dbInitializer.Initialize();
    ...
}
```
