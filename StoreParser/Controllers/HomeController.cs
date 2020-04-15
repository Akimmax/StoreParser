using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreParser.Business;
using StoreParser.Dtos;
using StoreParser.Models;

namespace StoreParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParseService _requestTypeService;
        private IConfiguration _configuration { get; }
        private bool _isAsync { get; }

        public HomeController(IParseService requestTypeService, IConfiguration configuration)
        {
            _requestTypeService = requestTypeService;
            _configuration = configuration;            
            _isAsync = Convert.ToBoolean(_configuration["RuningMode:Async"]);
        }

        public async Task<IActionResult> Parse()
        {
            return View();
        }

        [HttpPost]       
        public async Task<IActionResult> Parse(ParsingConfiguration config)
        {
            try
            {
                IEnumerable<NewItemDto> newItems;
                if (_isAsync)//TODO Rework if statement
                {
                    newItems = await _requestTypeService.AddItemsAsync(config);
                }
                else
                {
                    newItems = await Task.Run(() => _requestTypeService.AddItems(config));
                }

                return View("NewItemsList", newItems);
            }
            catch (Exception e)
            {
                ViewData["Message"] = "A  exception has occurred";
                return View("NewItemsList", new List<NewItemDto>());
            }           
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<ItemDto> resualt;
            if (_isAsync)//TODO Rework if statement
            {
                resualt = await _requestTypeService.GetAllAsync();
            }
            else
            {
                resualt = await Task.Run(() => _requestTypeService.GetAll());
            }
            
            return View(resualt);
        }

        public async Task<IActionResult> Details(int id)
        {
            ItemDto resualt;
            if (_isAsync)//TODO Rework if statement
            {
                resualt = await _requestTypeService.GetByIdAsync(id);
            }
            else
            {
                resualt = await Task.Run(() => _requestTypeService.GetById(id));
            }

            return View(resualt);
        }
    }
}
