using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Context.Entities;
using Application.EntitiesServices.Interfaces;
using Data.Enumerations;
using System.Net;
using TajimaSoftwareTest.Models;
using Azure;
using Web.Helpers;

namespace Web.Controllers
{
    public class DesignsController : Controller
    {
        private readonly IDesignService _designService;

        public DesignsController(IDesignService designService)
        {
            _designService = designService;
        }

        // GET: Designs
        public async Task<IActionResult> Index()
        {
            var response = await _designService.GetAll();
            if(response.Response == HttpStatusCode.OK && response.Data is not null)
                return View(response.Data);
            else
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
        }

        // GET: Designs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var response = await _designService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    return View(response.Data);
                }
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        // GET: Designs/Create
        public IActionResult Create()
        {
            ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList();
            return View();
        }

        // POST: Designs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DecorationMethod,Name,Width,Height")] Design design)
        {
            if (ModelState.IsValid)
            {
                var response = await _designService.Create(design);
                if(response.Response == HttpStatusCode.Created)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            return RedirectToAction(nameof(Error), new { message = "Model State is not valid", status = HttpStatusCode.BadRequest });
        }

        // GET: Designs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var response = await _designService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response?.Data is not null)
                {
                    ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList(response.Data.DecorationMethod);
                    return View(response.Data);
                }
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        // POST: Designs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DecorationMethod,Name,Width,Height")] Design design)
        {
            if (id != design.Id)
            {
                RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
            }
            
            if (ModelState.IsValid)
            {
                var response = await _designService.Update(id, design);
                if (response.Response == HttpStatusCode.OK)
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList(design.DecorationMethod);
            return View(design);
        }

        // GET: Designs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var response = await _designService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                    return View(response.Data);
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            else
                return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        // POST: Designs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _designService.Delete(id);
            if (response.Response == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
        }

        public IActionResult Error(string message, HttpStatusCode status)
        {
            var errorModel = new ErrorViewModel
            {
                ErrorMessage = message,
                StatusCode = status
            };
            return View(errorModel);
        }

        
    }
}

