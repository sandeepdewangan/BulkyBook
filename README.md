# ASP.NET Core 

## Install Packages

1. Markdown Editor - README.md file editor.
2. Runtime Compilation

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