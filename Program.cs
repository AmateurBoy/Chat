using ChatMarchenkoIlya;
using ChatMarchenkoIlya.Data;

using System.Web;
using System.Drawing;
using Microsoft.Net.Http.Headers;
using ChatMarchenkoIlya.Services;
using ChatMarchenkoIlya.Controllers;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSignalR();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Index}/{action=Index}");

});


//ImgGeneration.DrawText("что то на ельфийском", font: new System.Drawing.Font("Arial", 24, FontStyle.Bold), textColor: System.Drawing.Color.Black, backColor: System.Drawing.Color.AliceBlue);


app.Run();


