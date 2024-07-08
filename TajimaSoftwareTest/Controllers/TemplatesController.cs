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
using Application.EntitiesServices;
using System.Net;
using Web.Helpers;
using Newtonsoft.Json;
using TajimaSoftwareTest.Models;

namespace Web.Controllers
{
    public class TemplatesController : Controller
    {
        ITemplateService _templateService;
        IDesignService _designService;

        public TemplatesController(ITemplateService templateService, IDesignService designService)
        {
            _templateService = templateService;
            _designService = designService;
        }

        // GET: Templates
        public async Task<IActionResult> Index()
        {
            var response = await _templateService.GetAll();
            if (response.Response == HttpStatusCode.OK && response.Data is not null)
                return View(response.Data);
            else
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
        }

        // GET: Templates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var response = await _templateService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    return View(response.Data);
                }
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        //GET: Templates/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList();
            var response = await _designService.GetAll();

            if (response.Response == HttpStatusCode.OK)
                ViewBag.AvailableDesigns = response.Data;
            else
                return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });

            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DecorationMethod,Name")] Template template, string designs)
        {
            if (ModelState.IsValid)
            {
                List<int> selectedDesignsIds = new List<int>();
                if (!string.IsNullOrEmpty(designs))
                {
                    try
                    {
                        selectedDesignsIds = JsonConvert.DeserializeObject<List<int>>(designs)!;
                    }
                    catch
                    {
                        return RedirectToAction(nameof(Error), new { message = "Error while deserialing", status = HttpStatusCode.InternalServerError });
                    }
                }
                var response = await _templateService.Create(template, selectedDesignsIds);
                if (response.Response == HttpStatusCode.Created)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            else
                return View();
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {

                var response = await _templateService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response?.Data is not null)
                {
                    ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList(response.Data.DecorationMethod);
                    var getRemainingResponse = await _templateService.GetRemainingDesigns(id.Value);
                    if (getRemainingResponse.Response == HttpStatusCode.OK)
                        ViewBag.RemainingDesigns = getRemainingResponse.Data;
                    return View(response.Data);
                }
                else
                    return RedirectToAction(nameof(Error)   , new { message = response.Error, status = response.Response });
            }
            return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DecorationMethod,Name")] Template template, string designs)
        {
            if (id != template.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
            }
            
            if (ModelState.IsValid)
            {
                List<int> selectedDesignsIds = new List<int>();
                if (!string.IsNullOrEmpty(designs))
                {
                    try
                    {
                        selectedDesignsIds = JsonConvert.DeserializeObject<List<int>>(designs)!;
                    }
                    catch
                    {
                        return RedirectToAction(nameof(Error), new { message = "Error while deserializing", status = HttpStatusCode.InternalServerError });
                    }
                }
                var response = await _templateService.Update(id, template, selectedDesignsIds);
                if (response.Response == HttpStatusCode.OK)
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }

            var getRemainingResponse = await _templateService.GetRemainingDesigns(id);
            if(getRemainingResponse.Response == HttpStatusCode.OK)
                ViewBag.RemainingDesigns = getRemainingResponse.Data;
            ViewBag.DecorationMethodList = HelperMethods.GetDecorationMethodSelectList(template.DecorationMethod);
            return View(template);
        }

        // GET: Templates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var response = await _templateService.GetById(id.Value);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                    return View(response.Data);
                else
                    return RedirectToAction(nameof(Error), new { message = response.Error, status = response.Response });
            }
            else
                return RedirectToAction(nameof(Error), new { message = "Invalid Id", status = HttpStatusCode.BadRequest });
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _templateService.Delete(id);
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

