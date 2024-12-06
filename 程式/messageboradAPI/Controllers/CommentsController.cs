using messageboradAPI.Models.Data;
using messageboradAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace messageboradAPI.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public CommentsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 顯示留言列表
        public async Task<IActionResult> Index()
        {
            //(API)顯示留言。使用 HttpClient 發送一個 GET 請求
            var response = await _httpClient.GetAsync("https://localhost:7167/api/APIComments");
            if (!response.IsSuccessStatusCode) //檢查 HTTP 響應的狀態碼
                return View("Error");

            var json = await response.Content.ReadAsStringAsync(); //讀取 HTTP 響應的內容（JSON 格式）並存儲為字串。
            var comments = JsonSerializer.Deserialize<List<Comment>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true //反序列化時忽略屬性名稱的大小寫差異
            }); // 整理數據成好顯示在網站上，將 JSON 字串轉換為一個 List<Comment> 對象，並自定義反序列化過程。

            /*
            反序列化過程

            JSON 解析器會檢查 JSON 中的每個對象。
            根據 Comment 類的屬性名稱匹配 JSON 的鍵名。
            將 JSON 數據映射到 Comment 類型的對象並存入 List<Comment>。
            */

            return View(comments);
        }

        // 提交新留言
        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            var json = JsonSerializer.Serialize(comment); //將 comment 對象序列化為 JSON 字串。
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // 整理成一包，將 JSON 字串封裝為 HttpContent 對象

            // 使用 HttpClient 發送 POST 請求
            var response = await _httpClient.PostAsync("https://localhost:7167/api/APIComments", content);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            return RedirectToAction("Index");
        }

        // 刪除留言
        public async Task<IActionResult> Delete(int id)
        {
            //(API)刪除留言
            var response = await _httpClient.DeleteAsync($"https://localhost:7167/api/APIComments/{id}");
            if (!response.IsSuccessStatusCode)
                return View("Error");

            return RedirectToAction("Index");
        }

        // 修改留言
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7167/api/APIComments/{id}");
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var comment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(comment); // 將特定留言傳遞到編輯視圖
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Comment comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7167/api/APIComments/{comment.Id}", content);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            return RedirectToAction("Index");
        }
    }
}