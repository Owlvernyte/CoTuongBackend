using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoTuongBackend.Application.Users
{
    public record LoginDTO
    {
        public required string UserNameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
