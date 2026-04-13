using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VocabularyTrainer.Data.Models
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;  // Default value to prevent null warnings
                                                             // 
        public int Count { get; set; }

        public UploadResult(bool success, object message)
        {
            Success = success;
            Message = message.ToString() ?? string.Empty;  // Ensure it's never null
            Count = success ? (int)message : 0;
        }
    }

}