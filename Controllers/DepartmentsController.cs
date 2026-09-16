using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Practice_Web_Api.Models;
using CRUD_Practice_Web_Api.Data;
using CRUD_Practice_Web_Api.DTOs;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DepartmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
    {
        return await _context.Departments.AsNoTracking().ToListAsync();
    }

    // GET: api/Department/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        return department;
    }

    // PUT: api/Department/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDepartment(int? id, DepartmentUpdateDto dto)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        if (await _context.Departments.AnyAsync(d => d.DepartmentName == dto.DepartmentName && d.Id != id))
        {
            return Conflict("Department name already exists.");
        }

        department.DepartmentName = dto.DepartmentName;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartmentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Department
    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment(DepartmentCreateDto dto)
    {
        if (await _context.Departments.AnyAsync(d => d.DepartmentName == dto.DepartmentName))
        {
            return Conflict("Department name already exists.");
        }

        var department = new Department
        {
            DepartmentName = dto.DepartmentName
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
    }

    // DELETE: api/Department/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int? id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DepartmentExists(int? id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}
