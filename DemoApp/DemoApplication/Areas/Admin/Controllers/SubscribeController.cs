using DemoApplication.Areas.Admin.ViewModels.Subscribe;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("subscribe")]
    public class SubscribeController : Controller
    {

        private readonly DataContext _dataContext;
        

        public SubscribeController(DataContext dataContext)
        {
            _dataContext = dataContext;
           
        }

        [HttpGet("list", Name = "admin-subscribe-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Subscribes.Select(s=> new ListViewModel(s.Id,s.Email, s.CreatedAt,s.UpdatedAt)).ToListAsync();

            return View(model);
        }
    }
}
