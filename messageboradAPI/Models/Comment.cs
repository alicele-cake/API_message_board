using System;
using System.ComponentModel.DataAnnotations;

namespace messageboardAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]//會根據模型中的 [StringLength] 和 [Required] 特性，自動生成了這些前端 HTML 屬性。
        public string? Username { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
