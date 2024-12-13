using messageboardAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// 添加 HttpClient，讓CommentsController能用API
builder.Services.AddHttpClient();

// 添加必需的服務
builder.Services.AddControllersWithViews(); // 支持控制器和視圖
builder.Services.AddControllers(); // 添加對 API 控制器的支持
builder.Services.AddAuthorization(); // 添加授權支持

// 配置 CommentContext（資料庫上下文）
//2
builder.Services.AddDbContext<CommentContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ).LogTo(Console.WriteLine));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // 預設的 HSTS 為 30 天
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// **映射 MVC 控制器（帶視圖的）**
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// **映射 API 控制器**
app.MapControllers(); // 確保 APICommentsController 的路由能夠被識別

app.Run();

