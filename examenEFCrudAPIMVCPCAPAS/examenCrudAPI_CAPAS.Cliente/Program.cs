using examenCrudAPI_CAPAS.Cliente.Services.Contratos;
using examenCrudAPI_CAPAS.Cliente.Services.Implementaciones;

var builder = WebApplication.CreateBuilder(args);

var baseURL = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IDepartamentoService, DepartamentoService>(client =>
    client.BaseAddress = new Uri(baseURL!)
);
builder.Services.AddHttpClient<IEmpleadoService, EmpleadoService>(client =>
    client.BaseAddress = new Uri(baseURL)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Empleado}/{action=Index}/{id?}");

app.Run();
