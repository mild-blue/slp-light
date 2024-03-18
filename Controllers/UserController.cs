using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using slp.light.DTO;
using slp.light.Interfaces;

namespace slp.light.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<UserOutDto>> Get([FromServices] IAppDbContext dbContext)
        {
            var mapper = MapperConfig.InitializeAutomapper();

            var users = dbContext.Users;

            return await mapper
                .ProjectTo<UserOutDto>(users, parameters: null)
                .ToListAsync();
        }
    }
}
