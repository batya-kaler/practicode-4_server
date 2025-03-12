
// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<ToDoDbContext>(options =>
// options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 41))));
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//             .AllowAnyHeader()
//             .AllowAnyMethod();
//     });
// });
// var app = builder.Build();
// app.UseCors("AllowAll");
// // if (builder.Environment.IsDevelopment())
// // {
//     app.UseSwagger();
//     app.UseSwaggerUI(options =>
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//         options.RoutePrefix = string.Empty;
//     });
// }
// app.MapGet("/", () => "Hello World!");
using TodoApi; 
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // נוסיף את הספרייה של Swagger

var builder = WebApplication.CreateBuilder(args);

// הוספת DbContext ושימוש במחרוזת חיבור מתוך appsettings.json
// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
// builder.Services.AddDbContext<ToDoDbContext>(options =>
// options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddDbContext<ToDoDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 25))));
// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDo API",
        Version = "v1",
        Description = "API לניהול משימות"
    });
});

builder.Services.AddCors(options =>
{
      options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

//  הפעלת Swagger
//if (app.Environment.IsDevelopment()) // מציג את Swagger רק בסביבת פיתוח
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
    });
//}

app.UseCors("AllowAll");

app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());

app.MapPost("/", async (ToDoDbContext db,string name) =>
{
    var item = new Item { Name = name, IsComplete = false };
    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return await db.Items.ToListAsync();
});
app.MapDelete("{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null)
        return Results.NotFound();
    db.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapPatch("{id}",async(ToDoDbContext db,int id,bool IsComplete) =>{

   var find=await db.Items.FindAsync(id);
    if(find==null)
    return Results.NotFound();
   find.IsComplete = IsComplete;
    await db.SaveChangesAsync();
return Results.Ok();
});
app.MapGet("/",()=>"success!!!");
app.Run();

