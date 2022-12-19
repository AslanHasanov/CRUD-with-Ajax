using DemoApplication.Areas.Admin.ViewModels.Author;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/author")]
    public class AuthorController : Controller
    {
        private readonly DataContext _dataContext;

        public AuthorController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-author-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = _dataContext.Authors
                .Select(a => new ListItemViewModel(a.Id, a.FirstName, a.LastName))
                .ToList();

            return View(model);
        }



        [HttpPost("add", Name = "admin-author-add")]
        public async Task<IActionResult> ListAsync(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newModel = new Author
            {
                FirstName = model.Name,
                LastName = model.LastName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            

            await _dataContext.Authors.AddAsync(newModel);

            var id= newModel.Id;    


            await _dataContext.SaveChangesAsync();

            return Created("admin-author-add", id);
        }



        [HttpDelete("delete/{id}", Name = "admin-author-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var model = await _dataContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (model is null)
            {
                return BadRequest();
            }
            _dataContext.Authors.Remove(model);
            await _dataContext.SaveChangesAsync();


     

            return NoContent();
        }

        [HttpGet("update/{id}", Name = "admin-author-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var model = await _dataContext.Authors.FirstOrDefaultAsync(b => b.Id == id);
            if (model is null)
            {
                return NotFound();
            }

            var newModel = new UpdateViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            return View(newModel);
        }

        [HttpPut("update/{id}", Name = "admin-author-update")]
        public async Task<IActionResult> UpdateAsync(UpdateViewModel model)
        {
            var author = await _dataContext.Authors.FirstOrDefaultAsync(a => a.Id == model.Id);


            if (author is null) return NotFound();
         

            if (!ModelState.IsValid) return View(model);
        

            author.FirstName = model.FirstName;
            author.LastName = model.LastName;
            author.UpdatedAt = DateTime.Now;


            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
