using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEnd_MobileShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public MobileController(MobileShopDbContext context)
        {
            _context = context;
        }
        // GET: api/mobile
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var mobiles = _context.Mobiles
                    .Include(m => m.Model)
                    .Select(m => new {
                        m.IMEINo,
                        m.Status,
                        m.Price,
                        m.ModelID,
                        ModelName = m.Model!.ModelName
                    })
                    .ToList();

                return Ok(mobiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách mobiles: {ex.Message}");
            }
        }

        // GET: api/mobile/{imei}
        [HttpGet("{imei}")]
        public IActionResult GetByIMEI(string imei)
        {
            try
            {
                var mobile = _context.Mobiles
                    .Include(m => m.Model)
                    .Where(m => m.IMEINo == imei)
                    .Select(m => new {
                        m.IMEINo,
                        m.Status,
                        m.Price,
                        m.ModelID,
                        ModelName = m.Model!.ModelName
                    })
                    .FirstOrDefault();

                if (mobile == null)
                    return NotFound("Không tìm thấy IMEI này.");

                return Ok(mobile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy mobile: {ex.Message}");
            }
        }

        // POST: api/mobile
        [HttpPost]
        public IActionResult Create([FromBody] Mobile mobile)
        {
            try
            {
                if (_context.Mobiles.Any(m => m.IMEINo == mobile.IMEINo))
                    return BadRequest("IMEI đã tồn tại.");

                _context.Mobiles.Add(mobile);
                _context.SaveChanges();

                return Ok($"Thêm mobile {mobile.IMEINo} thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi thêm mobile: {ex.Message}");
            }
        }

        // PUT: api/mobile/{imei}
        [HttpPut("{imei}")]
        public IActionResult Update(string imei, [FromBody] Mobile updated)
        {
            try
            {
                var mobile = _context.Mobiles.Find(imei);
                if (mobile == null)
                    return NotFound("Không tìm thấy mobile.");

                mobile.Status = updated.Status;
                mobile.Price = updated.Price;
                mobile.ModelID = updated.ModelID;

                _context.SaveChanges();

                return Ok($"Cập nhật mobile {imei} thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật mobile: {ex.Message}");
            }
        }

        // DELETE: api/mobile/{imei}
        [HttpDelete("{imei}")]
        public IActionResult Delete(string imei)
        {
            try
            {
                var mobile = _context.Mobiles.Find(imei);
                if (mobile == null)
                    return NotFound("Không tìm thấy mobile.");

                _context.Mobiles.Remove(mobile);
                _context.SaveChanges();

                return Ok($"Xoá mobile {imei} thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xoá mobile: {ex.Message}");
            }
        }

        [HttpGet("bymodel/{modelId}")]
        public IActionResult GetMobilesByModel(int modelId)
        {
            try
            {
                var mobiles = _context.Mobiles
                .Where(m => m.ModelID == modelId && m.Status == "Available")
                .Select(m => new {
                    m.IMEINo,
                    m.Price,
                    m.Status
                })
                .ToList();

                return Ok(mobiles);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
