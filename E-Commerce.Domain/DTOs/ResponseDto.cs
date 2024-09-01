using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public string? DisplayMessage { get; set; }
        public bool IsSucceeded { get; set; }
        public object? Result { get; set; }
        public List<string> ErrorMessages { get; set; }
        public ICollection<object>? Models { get; set; }

    }
}
