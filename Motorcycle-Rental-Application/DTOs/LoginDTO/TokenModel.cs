using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.DTOs.LoginDTO
{
    public class TokenModel
    {
        public string? AcessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
