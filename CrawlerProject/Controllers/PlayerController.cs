using CrawlerProject.Models.DTO;
using CrawlerProject.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace CrawlerProject.Controllers
{
    public class PlayerController : Controller
    {
        private readonly PlayerService _playerService;
        private readonly TeamService _teamService;
        public PlayerController(PlayerService playerService, TeamService teamService)
        {
            _playerService = playerService;
            _teamService = teamService;
        }
        public IActionResult Index()
        {
            var players = _playerService.GetListPlayer();
            var teams = _teamService.GetListTeam();
            var vm = new PlayerViewModel
            {
                Players = players,
                Teams = teams
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddPlayer([FromBody] AddPlayer model)
        {
            var result = _playerService.AddPlayer(model);
            return Json(result);
        }

        [HttpPost]
        public IActionResult DeletePlayer([FromBody] ObjectItem data)
        {
            return Ok(_playerService.Delete(data));
        }

        [HttpGet]
        public JsonResult GetPlayer([FromQuery] Guid id)
        {
            return Json(_playerService.GetPlayerById(new ObjectItem { Id = id }));
        }


        [HttpPost]
        public IActionResult UpdatePlayer([FromBody] UpdatePlayer model)
        {
            return Ok(_playerService.UpdatePlayer(model));
        }
    }
}
