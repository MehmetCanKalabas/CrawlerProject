using CrawlerProject.Models.DTO;
using CrawlerProject.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace CrawlerProject.Controllers
{
    public class TeamController : Controller
    {
        private readonly TeamService _teamService;
        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }
        public IActionResult Index()
        {
            var values = _teamService.GetListTeam();
            return View(values);
        }

        [HttpGet]
        public JsonResult GetAllTeams()
        {
            return Json(new { data = _teamService.GetListTeam() });
        }

        [HttpGet]
        public IActionResult AddTeam()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateTeamAsync([FromForm] AddOrUpdateTeamModel model)
        {
            var itemGuid = _teamService.AddOrUpdateTeam(model);

            await UploadImage(model.Logo, itemGuid ?? Guid.NewGuid());
            if (model.Logo == null)
            {
                return Json(new { itemGuid });
            } 
            var newFileName = itemGuid.ToString() + Path.GetExtension(model.Logo.FileName);
            var filePath = "uploads/" + newFileName;
            
            return Json(new { itemGuid, filePath });
        }

        private async Task<bool> UploadImage(IFormFile image, Guid itemGuid)
        {
            // Validate the inputs
            if (image == null || image.Length == 0)
                return false;

            try
            {
                // Create a new directory to save the file, if it doesn't exist.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var newFileName = itemGuid.ToString() + Path.GetExtension(image.FileName);

                // Save the file to the server
                var filePath = Path.Combine(path, newFileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // TODO: Save or process the email and file path as needed

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpDelete]
        public IActionResult DeleteTeam([FromBody] DeleteTeamModel model)
        {
            _teamService.DeleteTeamById(model.Id);
            return Json(new { success = true, message = "Team deleted successfully" });
        }
    }
}
