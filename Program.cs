
// using TodoApi; 
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models; 
// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<ToDoDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 25))));
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "ToDo API",
//         Version = "v1",
//         Description = "API לניהול משימות"
//     });
// });

// builder.Services.AddCors(options =>
// {
//       options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });

// var app = builder.Build();
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
//     });
// app.UseCors("AllowAll");
// app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());
// app.MapPost("/", async (ToDoDbContext db,string name) =>
// {
//     var item = new Item { Name = name, Iscomplete = false };
//     await db.Items.AddAsync(item);
//     await db.SaveChangesAsync();
//     return await db.Items.ToListAsync();
// });
// app.MapDelete("{id}", async (ToDoDbContext db, int id) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null)
//         return Results.NotFound();
//     db.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.Ok();
// });
// app.MapPatch("{id}",async(ToDoDbContext db,int id,bool IsComplete) =>{

//    var find=await db.Items.FindAsync(id);
//     if(find==null)
//     return Results.NotFound();
//    find.Iscomplete = IsComplete;
//     await db.SaveChangesAsync();
// return Results.Ok();
// });
// app.MapGet("",()=>"success!!!!!");
// app.Run();


using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), new MySqlServerVersion(new Version(8, 0, 25))));
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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
});
app.UseCors("AllowAll");

// Redirect from the root path to /items
app.MapGet("/", () => Results.Redirect("/items"));

app.MapGet("/items", async (ToDoDbContext db) => await db.Items.ToListAsync());

app.MapPost("/", async (ToDoDbContext db, string name) =>
{
    var item = new Item { Name = name, Iscomplete = false };
    await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return await db.Items.ToListAsync();
});

app.MapDelete("/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null)
        return Results.NotFound();
    db.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPatch("/{id}", async (ToDoDbContext db, int id, bool IsComplete) =>
{
    var find = await db.Items.FindAsync(id);
    if (find == null)
        return Results.NotFound();
    find.Iscomplete = IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();