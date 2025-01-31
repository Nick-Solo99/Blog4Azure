using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.Areas.Identity.Pages.Account;

public class Admin : PageModel
{
    private readonly UserManager<BlogUser> _userManager;

    public List<UserWithRoles> Users { get; set; } = new();
    
    [BindProperty]
    public string SelectedUserId { get; set; }

    public Admin(UserManager<BlogUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task OnGetAsync()
    {
        foreach (var user in _userManager.Users.ToList())
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            Users.AddRange(new UserWithRoles
            {
                User = user,
                Roles = roles.ToList()
            });
        }
        
    }

    public async Task<IActionResult> OnPostAddAdminAsync()
    {
        var user = await _userManager.FindByIdAsync(SelectedUserId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostRemoveAdminAsync()
    {
        var user = await _userManager.FindByIdAsync(SelectedUserId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostAddContributorAsync()
    {
        var user = await _userManager.FindByIdAsync(SelectedUserId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, "Contributor");
        }
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostRemoveContributorAsync()
    {
        var user = await _userManager.FindByIdAsync(SelectedUserId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, "Contributor");
        }
        return RedirectToPage();
    }

    public class UserWithRoles
    {
        public BlogUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}