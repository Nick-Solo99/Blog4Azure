using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers;

[Authorize]
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;

    public ArticleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Article
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }

    // GET: Article/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles
            .Include(a => a.Contributor)
            .FirstOrDefaultAsync(m => m.ArticleId == id);
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    [Authorize(Roles = "Contributor")]
    // GET: Article/Create
    public IActionResult Create()
    {
        ViewData["ContributorEmail"] = User.Identity!.Name;
        return View();
    }

    // POST: Article/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Contributor")]
    public async Task<IActionResult> Create([Bind("ArticleId,Title,Body,StartDate,EndDate")] Article article)
    {
        if (ModelState.IsValid)
        {
            article.CreateDate = DateTime.UtcNow;
            article.ContributorEmail = User.Identity!.Name;
            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ContributorEmail"] = User.Identity!.Name;
        return View(article);
    }

    [Authorize(Roles = "Contributor")]
    // GET: Article/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles.FindAsync(id);
        if (article == null)
        {
            return NotFound();
        }
        ViewData["ContributorEmail"] = User.Identity!.Name;
        return View(article);
    }

    [Authorize(Roles = "Contributor")]
    // POST: Article/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Body,CreateDate,StartDate,EndDate,ContributorEmail")] Article article)
    {
        if (id != article.ArticleId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(article.ArticleId))
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
        ViewData["ContributorEmail"] = new SelectList(_context.Users, "Email", "Id", article.ContributorEmail);
        return View(article);
    }

    [Authorize(Roles = "Contributor")]
    // GET: Article/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await _context.Articles
            .Include(a => a.Contributor)
            .FirstOrDefaultAsync(m => m.ArticleId == id);
        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }

    [Authorize(Roles = "Contributor")]
    // POST: Article/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.ArticleId == id);
    }
}