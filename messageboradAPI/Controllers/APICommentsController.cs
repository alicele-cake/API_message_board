using messageboardAPI.Models.Data;
using messageboardAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using messageboardAPI.Models;

namespace messageboardAPI.Controllers
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
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var query = "SELECT * FROM Comments ORDER BY CreatedAt";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            await _context.Database.OpenConnectionAsync();

            var reader = await command.ExecuteReaderAsync();
            var comments = await DataReaderExtensions2.MapToCommentsAsync(reader);

            return Ok(comments);
        }

        // GET: api/APIComments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            Comment comment = null;
            var query = "SELECT * FROM Comments WHERE Id = @Id";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            await _context.Database.OpenConnectionAsync();

            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                comment = new Comment
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Message = reader.GetString(reader.GetOrdinal("Message")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid) // 驗證模型資料是否有效
            {
                return BadRequest(ModelState); // 回傳 HTTP 400 狀態碼與驗證錯誤訊息
            }

            comment.CreatedAt = DateTime.Now;

            var query = @"
        INSERT INTO Comments (Username, Message, CreatedAt)
        OUTPUT INSERTED.Id
        VALUES (@Username, @Message, @CreatedAt);";

            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;

            command.Parameters.Add(CreateParameter(command, "@Username", comment.Username, DbType.String));
            command.Parameters.Add(CreateParameter(command, "@Message", comment.Message, DbType.String));
            command.Parameters.Add(CreateParameter(command, "@CreatedAt", comment.CreatedAt, DbType.DateTime));

            await _context.Database.OpenConnectionAsync();

            var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
            comment.Id = newId;

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
                return BadRequest();

            if (!ModelState.IsValid) // 驗證模型資料是否有效
            {
                return BadRequest(ModelState);
            }

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

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
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
