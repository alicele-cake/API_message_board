using messageboradAPI.Models.Data;
using messageboradAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;


namespace messageboradAPI.Controllers
{

    //  api/APIComments
    [Route("api/[controller]")]
    [ApiController]
    public class APICommentsController : ControllerBase
    {
        private readonly CommentContext _context;

        public APICommentsController(CommentContext context)
        {
            _context = context;
        }

        // GET: api/APIComments
        //  https://localhost:7167/api/APIComments
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            
            var query = "SELECT * FROM Comments ORDER BY CreatedAt";

            await using var command = _context.Database.GetDbConnection().CreateCommand();  //啟用資料庫
            command.CommandText = query; //輸入命令
            await _context.Database.OpenConnectionAsync(); //開啟資料庫連接

            var reader = await command.ExecuteReaderAsync();  //執行命令取得查詢結果
            var comments = await DataReaderExtensions2.MapToCommentsAsync(reader); //DbDataReader的查詢結果轉換成一個 List<Comment>

            return Ok(comments);
            
        }

        // GET: api/APIComments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var query = "SELECT * FROM Comments WHERE Id = @Id";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            command.Parameters.Add(CreateParameter(command, "@Id", id, DbType.Int32)); //@Id參數設定成(DbType.int)id

            await _context.Database.OpenConnectionAsync();

            var reader = await command.ExecuteReaderAsync();
            var comments = await DataReaderExtensions2.MapToCommentsAsync(reader);

            if (comments == null || comments.Count == 0) // 檢查是否有結果
                return NotFound(); //回傳一個 HTTP 404 Not Found 狀態碼

            return Ok(comments);
        }


        // POST: api/APIComments
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)//將 HTTP 請求主體中 JSON 格式的資料反序列化為 Comment 類型的物件
        {
            comment.CreatedAt = DateTime.Now;

            var query = @"
        INSERT INTO Comments (Username, Message, CreatedAt)
        OUTPUT INSERTED.Id
        VALUES (@Username, @Message, @CreatedAt);";//OUTPUT INSERTED.Id 查詢並返回插入記錄的主鍵(Id)

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            command.Parameters.Add(CreateParameter(command, "@Username", comment.Username, DbType.String));
            command.Parameters.Add(CreateParameter(command, "@Message", comment.Message, DbType.String));
            command.Parameters.Add(CreateParameter(command, "@CreatedAt", comment.CreatedAt, DbType.DateTime));

            await _context.Database.OpenConnectionAsync();

            // 使用 ExecuteScalar 獲取 INSERTED.Id 的值
            var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
            comment.Id = newId;

            //回傳路由並生成:api/APIComments/{id}
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
            //nameof(GetComment)：用來指定CreatedAtAction生成方式的規則，像這裡是:api/APIComments/{id}。
            //new 關鍵字表示建立一個新物件，提供路由參數值(id)
            //comment：新建的資源資料

        }


        // PUT: api/APIComments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
                return BadRequest(); //HTTP 400 狀態碼表示用戶端請求無效（Bad Request）

            var query = @"
                UPDATE Comments
                SET Username = @Username, Message = @Message
                WHERE Id = @Id";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            command.Parameters.Add(CreateParameter(command, "@Id", id, DbType.Int32));
            command.Parameters.Add(CreateParameter(command, "@Username", comment.Username, DbType.String));
            command.Parameters.Add(CreateParameter(command, "@Message", comment.Message, DbType.String));

            await _context.Database.OpenConnectionAsync();

            var rowsAffected = await command.ExecuteNonQueryAsync();//用於執行不返回查詢結果的 SQL 語句，返回的結果是影響的資料行數
            if (rowsAffected == 0)
                return NotFound();//HTTP 狀態碼：404，表示伺服器無法找到請求的資源。

            return NoContent(); //HTTP 狀態碼：204，表示請求成功，但伺服器不需要返回任何內容。
        }

        // DELETE: api/APIComments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var query = "DELETE FROM Comments WHERE Id = @Id";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            command.Parameters.Add(CreateParameter(command, "@Id", id, DbType.Int32));

            await _context.Database.OpenConnectionAsync();

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
        }

        // 簡化參數化查詢的參數建立過程。我本來就想寫的條件式過濾SQL參數
        private static DbParameter CreateParameter(DbCommand command, string name, object value, DbType type)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            parameter.DbType = type;
            return parameter;
        }
    }

}
