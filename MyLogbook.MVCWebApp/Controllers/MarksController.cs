using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLogbook.AppContext;
using MyLogbook.Entities;

namespace MyLogbook.MVCWebApp.Controllers
{
    public class MarksController : Controller
    {
        private readonly AppDbContext _context;

        public MarksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marks.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create(Guid id)
        {
			var mark = new Mark();
			mark.Date = DateTime.Now;			
			mark.Student = _context.Students.Include(s=>s.Group).Include(s=>s.Marks).FirstOrDefault(s=>s.Id == id);
			var subjects = _context.Set<Subject>();
			ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
			var teachers = _context.Set<Teacher>();
			ViewBag.Teachers = new SelectList(teachers, "Id", "LastName");
			return View(mark);
        }

        // POST: Marks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid Subject,Guid Teacher, Guid Id, [Bind("Value,Date,Id")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                mark.Id = Guid.NewGuid();
				mark.Student = _context.Students.Include(s => s.Group).Include(s => s.Marks).FirstOrDefault(s=>s.Id == Id);
				mark.Subject = _context.Subjects.Include(s=>s.Faculty).Include(s=>s.Marks).FirstOrDefault(s=>s.Id == Subject);
				mark.Teacher = _context.Teachers.Include(s => s.Subjects).Include(s => s.Marks).FirstOrDefault(s => s.Id == Teacher);
				_context.Add(mark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Value,Date,Id")] Mark mark)
        {
            if (id != mark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mark = await _context.Marks.FindAsync(id);
            _context.Marks.Remove(mark);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(Guid id)
        {
            return _context.Marks.Any(e => e.Id == id);
        }
    }
}
