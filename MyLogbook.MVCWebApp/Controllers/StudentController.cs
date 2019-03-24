using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLogbook.Entities;
using MyLogbook.Repositories;

namespace MyLogbook.MVCWebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _repository;

        public StudentController(IStudentRepository repository)
        {
            _repository = repository;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _repository.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _repository.GetItemAsync(id.Value);
			var student = await _repository.AllItems
				.Include(s => s.Marks)
					.ThenInclude(m=>m.Subject)
				.Include(s => s.Group)				
				.FirstOrDefaultAsync(x => x.Id == id.Value);			
			if (student == null)
            {
                return NotFound();
            }					 			
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
			var groups = _repository.Context.Set<Group>();
			ViewBag.Groups = new SelectList(groups, "Id", "Name");
			return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid Group, [Bind("FirstName,LastName,Id")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid();
				student.Group = _repository.Context.Find<Group>(Group);
                await _repository.AddItemAsync(student);

                return RedirectToAction(nameof(Index));
            }
			
			return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var student = await _repository.AllItems.Include(s => s.Group).FirstOrDefaultAsync(x => x.Id == id.Value);
			if (student == null)
            {
                return NotFound();
            }
			var groups = _repository.Context.Set<Group>();
			ViewBag.Groups = new SelectList(groups, "Id", "Name");
			return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Group, Guid id, [Bind("FirstName,LastName,Id")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {				
				student.Group = _repository.Context.Find<Group>(Group);
                if (!await _repository.ChangeItemAsync(student))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _repository.GetItemAsync(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
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
