using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("basket")]
    public class BasketController : Controller
    {
        private readonly DataContext _datacontext;

        public BasketController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }


        [HttpGet("add/{id}", Name = "client-basket-add")]
        public async Task<IActionResult> AddProductAsync([FromRoute] int id)
        {
            var product = await _datacontext.Books.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var booksViewModel = new List<ProductCookieViewModel>();

            var productCookieValue = HttpContext.Request.Cookies["products"];


            if (productCookieValue == null)
            {
                 booksViewModel = new List<ProductCookieViewModel>
               {
                   new ProductCookieViewModel(product.Id, product.Title, string.Empty, 1, product.Price, product.Price)
               };

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(booksViewModel));

            }
            else
            {
                 booksViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);

                var targetBookCookie = booksViewModel.FirstOrDefault(tb => tb.Id == id); // var olan kitabi cokienin icinde axtarmaq

                if (targetBookCookie == null) // cokienin icin de var olan kitab yoxdursa 
                {
                    booksViewModel.Add(new ProductCookieViewModel(product.Id, product.Title, string.Empty, 1, product.Price, product.Price));
                }
                else // cokienin icin de var olan kitab varsa  
                {
                    targetBookCookie.Quantity += 1;
                    targetBookCookie.Total = targetBookCookie.Quantity * targetBookCookie.Price;

                }

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(booksViewModel));

            }

            return ViewComponent(nameof(ShopCart), booksViewModel);
        }

        [HttpGet("delete/{id}", Name = "client-basket-delete")]

        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var product = await _datacontext.Books.FirstOrDefaultAsync(tb => tb.Id == id);

            if (product == null)
            {
                return NotFound();
            }


            var productsCookieViewModel = new List<ProductCookieViewModel>();

            var productCookieValue = HttpContext.Request.Cookies["products"];

            if (productCookieValue == null)
            {
                return NotFound();

            }


            productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);

            foreach(var item in productsCookieViewModel)
            {
                if(item.Quantity>1)
                {
                    item.Quantity--;
                    item.Total = item.Quantity * item.Price;
                }
                else
                {
                    productsCookieViewModel.Remove(item);
                    break;
                }
            }


            HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

            return ViewComponent(nameof(ShopCart), productsCookieViewModel);
        }

    }
}
