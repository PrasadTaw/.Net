

using DAL.Connected;
using BOL;

var builder = WebApplication.CreateBuilder(args);

//*************************************************************
// Install the Microsoft.AspNetCore.Cors Nuget package.

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware Configuration
//Set HTTP pipeline (middleware Pipeline)

app.UseHttpsRedirection();
app.UseCors(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
});
app.UseAuthorization();
app.MapControllers();


//*****Minimal web API with ASP.NET Core *******

app.MapGet("/api/employees",() =>
      {
        return Results.Ok(DBManager.GetAllEmployees());    
});
 
app.MapGet("/api/employees/{id}",(int id) =>
    {
    bool status=true; 
    var emp=new Employee{ Id=id, FirstName="Ravi", LastName="Tambade"};
    
    if(status){
        return Results.Ok(emp);
    }      
    return Results.NotFound();
});

app.MapPost("/api/employees",(Employee emp) =>
{
    DBManager.Insert(emp);
    return Results.Created($"/employees/{emp.Id}", emp);
});

app.MapPut("/api/employees/{id}",   (int id, Employee emp) =>
{
    var existingEmp =  new Employee{ Id=id, FirstName="Ravi", LastName="Tambade"};  ;
    if (existingEmp is null) return Results.NotFound();
    existingEmp.FirstName = emp.FirstName;
    existingEmp.LastName = emp.LastName;
    return Results.NoContent();
});

app.MapDelete("/api/employees/{id}",   (int id) =>
{
    bool status= false;
    if (status){
        return Results.Ok();
    }
     return Results.NotFound();
});



app.Run();
