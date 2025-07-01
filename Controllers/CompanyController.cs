using BackEnd_MobileShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public CompanyController(MobileShopDbContext context)
        {
            _context = context;
        }

        // GET: api/company
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var companies = _context.Companies.ToList();
                return Ok(new
                {
                    message = "Danh sách company:",
                    companies = companies
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách company: {ex.Message}");
            }
        }

        // GET: api/company/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var company = _context.Companies.Find(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy company: {ex.Message}");
            }

        }

        // POST: api/company
        [HttpPost]
        public IActionResult Create([FromBody] Company company)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Companies.Add(company);
                _context.SaveChanges();

                return Ok(new
                {
                    message = $"Thêm công ty {company.CompanyName} thành công!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi thêm công ty: {ex.Message}");
            }
        }

        // PUT: api/company/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Company updatedCompany)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var company = _context.Companies.Find(id);
                if (company == null)
                    return NotFound();

                company.CompanyName = updatedCompany.CompanyName;

                _context.SaveChanges();
                return Ok(new
                {
                    message = $"Sửa công ty {company.CompanyName} thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi sửa thông tin công ty: {ex.Message}");
            }
        }

        // DELETE /api/company/{id}
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var company = _context.Companies.FirstOrDefault(c => c.CompanyID == id);
                if (company == null)
                {
                    return NotFound($"Không tồn tại công ty {id}");
                }
                _context.Companies.Remove(company);
                _context.SaveChanges();
                return Ok(new
                {
                    message = "Xóa công ty thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa công ty: {ex.Message}");
            }
        }

        // GET: api/company/nextid
        [HttpGet("nextid")]
        public IActionResult GetNextCompanyId()
        {
            try
            {
                int nextId = _context.Companies.Any()
                ? _context.Companies.Max(c => c.CompanyID) + 1
                : 1;
                return Ok(nextId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
