using messageboardAPI.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// �K�[ HttpClient�A��CommentsController���API
builder.Services.AddHttpClient();

// �K�[���ݪ��A��
builder.Services.AddControllersWithViews(); // �������M����
builder.Services.AddControllers(); // �K�[�� API ��������
builder.Services.AddAuthorization(); // �K�[���v���

// �t�m CommentContext�]��Ʈw�W�U��^
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
    // �w�]�� HSTS �� 30 ��
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// **�M�g MVC ����]�a���Ϫ��^**
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// **�M�g API ���**
app.MapControllers(); // �T�O APICommentsController �����ѯ���Q�ѧO

app.Run();

