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