using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeGen.Result.Models;

namespace CodeGen.Result.Controllers;

/// <summary>
/// This code is automatically generated by CodeGen Api command
/// </summary>
public class SampleTableController : Controller
{
    private readonly SampleContext _context;

    public SampleTableController(SampleContext context)
    {
        _context = context;
    }

    // GET: SampleTable
    public async ValueTask<IActionResult> Index()
    {
        return View(await _context.SampleTables.ToListAsync());
    }

    // GET: SampleTable/Details/5
    public async ValueTask<IActionResult> Details(int id)
    {
        var target = _context.SampleTables.AsNoTracking();
        target = target.Where(x => x.Id == id);
        var result = await target.SingleOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    // GET: SampleTable/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SampleTable/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async ValueTask<IActionResult> Create([Bind("Id,TargetName,TargetInt,TargetDecimal,TargetDate,TargetBit,CreateAt,CreateUser,UpdateAt,UpdateUser")] SampleTable target)
    {
        if (ModelState.IsValid)
        {
            _context.Add(target);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(target);
    }

    // GET: SampleTable/Edit/5
    public async ValueTask<IActionResult> Edit(int id)
    {
        var target = _context.SampleTables.AsNoTracking();
        target = target.Where(x => x.Id == id);
        var result = await target.SingleOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }
        return View(result);
    }

    // POST: SampleTable/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async ValueTask<IActionResult> Edit(int id, [Bind("Id,TargetName,TargetInt,TargetDecimal,TargetDate,TargetBit,CreateAt,CreateUser,UpdateAt,UpdateUser")] SampleTable target)
    {
        if (target.Id != id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(target);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
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
        return View(target);
    }

    // GET: SampleTable/Delete/5
    public async ValueTask<IActionResult> Delete(int id)
    {
        var target = _context.SampleTables.AsNoTracking();
        target = target.Where(x => x.Id == id);
        var result = await target.SingleOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    // POST: SampleTable/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async ValueTask<IActionResult> DeleteConfirmed(int id)
    {
        var target = _context.SampleTables.AsNoTracking();
        target = target.Where(x => x.Id == id);
        var result = await target.SingleOrDefaultAsync();

        if (result != null)
        {
            _context.SampleTables.Remove(result);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Exists(int id)
    {
        var target = _context.SampleTables.AsNoTracking();
        target = target.Where(x => x.Id == id);
        return target.Any();
    }
}
