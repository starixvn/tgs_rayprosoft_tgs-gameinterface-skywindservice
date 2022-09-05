using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices
{
    public record PlayerKickCommand(
        Uri ApiUrl,
        string SecretKey,
        string Username,
        string Password,
        string PlayerCode);
}