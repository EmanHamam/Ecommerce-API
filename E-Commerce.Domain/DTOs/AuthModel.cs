using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class AuthModel
    {
       
        public string? Message { get; set; }

        public bool IsAuthenticated { get; set; } = false;
        public string? UserName { get; set; }
        public int? CartID { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }

        //public DateTime TokenExpiration { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }  

    }
}
