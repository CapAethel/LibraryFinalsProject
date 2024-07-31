using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the database context with dependency injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    SeedData(context);
}

void SeedData(ApplicationDbContext context)
{
    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { CategoryName = "Fiction" },
            new Category { CategoryName = "Non-Fiction" },
            new Category { CategoryName = "Science Fiction" },
            new Category { CategoryName = "Fantasy" }
        );

        context.SaveChanges();
    }

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book { Title = "Book 1", Author = "Author 1", CategoryId = 1 },
            new Book { Title = "Book 2", Author = "Author 2", CategoryId = 2 },
            new Book { Title = "Book 3", Author = "Author 3", CategoryId = 3 },
            new Book { Title = "Book 4", Author = "Author 4", CategoryId = 4 },
            new Book { Title = "Book 5", Author = "Author 1", CategoryId = 1 },
            new Book { Title = "Book 6", Author = "Author 2", CategoryId = 2 },
            new Book { Title = "Book 7", Author = "Author 3", CategoryId = 3 },
            new Book { Title = "Book 8", Author = "Author 4", CategoryId = 4 }
        );

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
