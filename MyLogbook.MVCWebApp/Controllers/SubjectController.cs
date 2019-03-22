using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLogbook.Entities;
using MyLogbook.Repositories;

namespace MyLogbook.MVCWebApp.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _repository;

        public SubjectController(ISubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
			var data = await _repository.AllItems.Include(i=> i.Faculty).ToListAsync();
			return View(data);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			var item = await _repository.AllItems.Include(s => s.Faculty).FirstOrDefaultAsync(x => x.Id == id.Value);
			if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

       
        public IActionResult Create()
        {
			var faculty = _repository.Context.Set<Faculty>();
			ViewBag.Faculties = new SelectList(faculty, "Id", "Name");
			return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid Faculty, [Bind("Name,Id")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.Id = Guid.NewGuid();
				subject.Faculty = _repository.Context.Find<Faculty>(Faculty);
                await _repository.AddItemAsync(subject);

                return RedirectToAction(nameof(Index));
            }
			
			return View(subject);
        }
		        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var item = await _repository.AllItems.Include(s => s.Faculty).FirstOrDefaultAsync(x => x.Id == id.Value);
			if (item == null)
            {
                return NotFound();
            }
			var faculties = _repository.Context.Set<Faculty>();
			ViewBag.Faculties = new SelectList(faculties, "Id", "Name");
			return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Faculty, Guid id, [Bind("Name,Id")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {				
				subject.Faculty = _repository.Context.Find<Faculty>(Faculty);
                if (!await _repository.ChangeItemAsync(subject))
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

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
		      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
