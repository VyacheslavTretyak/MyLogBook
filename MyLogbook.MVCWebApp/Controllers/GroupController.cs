using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLogbook.Entities;
using MyLogbook.Repositories;

namespace MyLogbook.MVCWebApp.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _repository;

        public GroupController(IGroupRepository repository)
        {
            _repository = repository;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var data = await _repository.AllItems.Include(group => group.Faculty).ToListAsync();
            return View(data);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _repository.AllItems.Include(group => group.Faculty).FirstOrDefaultAsync(x => x.Id==id.Value);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
			var faculties = _repository.Context.Set<Faculty>();
			ViewBag.Faculties = new SelectList(faculties, "Id", "Name");
			return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid Faculty, [Bind("Name,Id")] Group item)
        {
			
            if (ModelState.IsValid)
            {
				item.Faculty = _repository.Context.Find<Faculty>(Faculty);
				item.Id = Guid.NewGuid();
                await _repository.AddItemAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var item = await _repository.GetItemAsync(id.Value);
			var item = await _repository.AllItems.Include(group => group.Faculty).FirstOrDefaultAsync(x => x.Id == id.Value);
			if (item == null)
            {
                return NotFound();
            }
			var faculties = _repository.Context.Set<Faculty>();
			ViewBag.Faculties = new SelectList(faculties, "Id", "Name");
			return View(item);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Faculty, Guid id, [Bind("Name,Id")] Group item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				item.Faculty = _repository.Context.Find<Faculty>(Faculty);
				if (!await _repository.ChangeItemAsync(item))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Group/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _repository.GetItemAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}