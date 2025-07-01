using BackEnd_MobileShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public ModelController(MobileShopDbContext context)
        {
            _context = context;
        }
        // GET: api/model
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var models = _context.Models
                .Include(m => m.Company)
                .Select(m => new
                {
                    m.ModelID,
                    m.ModelName,
                    m.AvailableQty,
                    m.CompanyID,
                    CompanyName = m.Company!.CompanyName 
                })
                .ToList();

                return Ok(new
                {
                    message = "Danh sách model",
                    models = models
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/model/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var model = _context.Models
                .Include(m => m.Company)
                .Where(m => m.ModelID == id)
                .Select(m => new
                {
                    m.ModelID,
                    m.ModelName,
                    m.AvailableQty,
                    m.CompanyID,
                    CompanyName = m.Company!.CompanyName
                })
                .FirstOrDefault();

                if (model == null)
                    return NotFound();

                return Ok(model);
            }

            catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/model
        [HttpPost]
        public IActionResult Create([FromBody] Model model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Models.Add(model);
                _context.SaveChanges();

                return Ok(new
                {
                    mesage = "Thêm model thành công.",
                    model = model
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/model/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Model updated)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var model = _context.Models.Find(id);
                if (model == null)
                    return NotFound();

                model.ModelName = updated.ModelName;
                model.AvailableQty = updated.AvailableQty;
                model.CompanyID = updated.CompanyID;

                _context.SaveChanges();
                return Ok("Cập nhật model thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/company/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var model = _context.Models.Find(id);
                if (model == null)
                    return NotFound();

                _context.Models.Remove(model);
                _context.SaveChanges();
                return Ok("Xoá model thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/company/bycompany/{companyId}
        [HttpGet("bycompany/{companyId}")]
        public IActionResult GetModelsByCompany(int companyId)
        {
            try
            {
                var models = _context.Models
                    .Where(m => m.CompanyID == companyId)
                    .Select(m => new {
                        m.ModelID,
                        m.ModelName
                    })
                    .ToList();
                return Ok(models);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/model/nextid
        [HttpGet("nextid")]
        public IActionResult GetNextModelId()
        {
            try
            {
                int nextId = _context.Models.Any()
                ? _context.Models.Max(m => m.ModelID) + 1
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
