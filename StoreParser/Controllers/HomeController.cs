using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreParser.Business;
using StoreParser.Dtos;
using StoreParser.Models;

namespace StoreParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParseService _requestTypeService;

        public HomeController(IParseService requestTypeService)
        {
            _requestTypeService = requestTypeService;
        }

        public IActionResult Parse()
        {
            return View();
        }

        [HttpPost]       
        public IActionResult Parse(ParsingConfiguration config)
        {
            try
            {
                var newItems = _requestTypeService.AddItems(config);
                return View("NewItemsList", newItems);
            }
            catch (Exception e)
            {
                ViewData["Message"] = "A  exception has occurred";
                return View("NewItemsList", new List<NewItemDto>());
            }           
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public IActionResult List()
        {
            var resualt = _requestTypeService.GetAll();
            return View(resualt);
        }

        public IActionResult Details(int id)
        {
            var resualt = _requestTypeService.GetById(id);
            return View(resualt);
        }     

    }
}
