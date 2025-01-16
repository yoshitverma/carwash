using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carwash.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carwash.Model;
using Microsoft.AspNetCore.Authorization;

namespace carwash.Controllers
{

[Route("api/packages")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PackageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Package>> GetPackages()
    {
        var packages = _context.Packages.Include(p => p.AddOns).ToList();
        return Ok(packages);
    }
}

}