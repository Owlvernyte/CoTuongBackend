using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoTuongBackend.Application.Chat.Dtos
{
    public sealed record ChatMessageDto(string RoomId,string Message);
}
