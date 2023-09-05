using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoTuongBackend.Application.Users
{
    public record ChagePasswordDTO
    {
        public required string UserNameOrEmail { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string OldPassword { get; set; }
    }
}
