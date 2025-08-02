using Microsoft.AspNetCore.Mvc;

namespace JobHubServer.Controllers
{
    public class WebSocketController : Controller
    {
        [Route("/ws")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

            }
        }
    }
}
