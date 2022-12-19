using DemoApplication.Areas.Client.ViewModels.Subscribe;

using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("subscribe")]
    public class SubscribeController : Controller
    {

        private readonly DataContext _dataContext;
        

        public SubscribeController(DataContext dataContext)
        {
            _dataContext = dataContext;
           
        }

        [HttpPost("add", Name = "client-subscribe-add")]
        public async Task< IActionResult> AddAsync(AddViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            if (_dataContext.Subscribes.Any(s=> s.Email == model.Email))
            {
                 ModelState.AddModelError(string.Empty, "Email alredy used");
                return BadRequest();
            }

            _dataContext.Subscribes.Add(new Subscribe
            {
                Email= model.Email,
                CreatedAt= DateTime.Now,
            });

           await _dataContext.SaveChangesAsync();

            return Ok();
        }

       
    }
}
