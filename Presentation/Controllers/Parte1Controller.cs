using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Interfaces.Services;

namespace ProvaPub.Presentation.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class Parte1Controller : ControllerBase
	{
		private readonly IRandomService _randomService;

		public Parte1Controller(IRandomService randomService)
		{
			_randomService = randomService;
		}

		[HttpGet]
		public async Task<int> Index()
		{
			return await _randomService.GetRandom();
		}
	}
}
