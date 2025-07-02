using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd_MobileShop.Models;
using BackEnd_MobileShop.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public SaleController(MobileShopDbContext context)
        {
            _context = context;
        }

        // POST: api/sale
        [HttpPost]
        public IActionResult CreateSale([FromBody] SaleRequest input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            try
            {
                // 1. Kiểm tra IMEINo còn hàng
                var mobile = _context.Mobiles.FirstOrDefault(m => m.IMEINo == input.IMEINo && m.Status == "Available");
                if (mobile == null)
                    return BadRequest("IMEINo không tồn tại hoặc đã bán.");

                // 2. Tạo customer nếu chưa có
                var customer = _context.Customers.FirstOrDefault(c =>
                    c.CustomerName == input.CustomerName && c.MobileNo == input.MobileNo);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        CustomerName = input.CustomerName,
                        MobileNo = input.MobileNo,
                        Email = input.Email,
                        Address = input.Address
                    };
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }

                // 3. Cập nhật máy đã bán
                mobile.Status = "Sold";

                // 4. Giảm tồn kho trong Models
                var model = _context.Models.FirstOrDefault(m => m.ModelID == mobile.ModelID);
                if (model != null)
                    model.AvailableQty--;

                // 5. Tạo Sale
                var sale = new Sale
                {
                    IMEINo = input.IMEINo,
                    SaleDate = DateTime.Now,
                    Price = mobile.Price,
                    CustomerID = customer.CustomerID
                };
                _context.Sales.Add(sale);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Bán hàng thành công.",
                    input
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi bán hàng: {ex.Message}");
            }
        }

        // GET: api/sale/date/{date}
        [HttpGet("date/{date}")]
        public IActionResult GetSalesByDate(DateTime date)
        {
            try
            {
                var sales = _context.Sales
                .Include(s => s.Mobile)
                    .ThenInclude(m => m!.Model)
                        .ThenInclude(model => model!.Company)
                .Include(s => s.Customer)
                .Where(s => s.SaleDate.Date == date.Date)
                .Select(s => new {
                    s.SaleID,
                    CompanyName = s.Mobile!.Model!.Company!.CompanyName,
                    ModelName = s.Mobile!.Model!.ModelName,
                    s.IMEINo,
                    s.Price
                })
                .ToList();

                if (sales.Count == 0)
                {
                    return NotFound("Không có đơn bán nào trong ngày.");
                }

                decimal totalPrice = sales.Sum(s => s.Price);
                return Ok(new
                {
                    sales,
                    TotalPriceInDate = totalPrice,
                });
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/sale/daterange?from=2024-07-01&to=2024-07-10
        [HttpGet("daterange")]
        public IActionResult GetSalesByDateRange(DateTime from, DateTime to)
        {
            try
            {
                var sales = _context.Sales
                .Include(s => s.Mobile)
                    .ThenInclude(m => m!.Model)
                        .ThenInclude(model => model!.Company)
                .Include(s => s.Customer)
                .Where(s => s.SaleDate.Date >= from.Date && s.SaleDate.Date <= to.Date)
                .Select(s => new {
                    s.SaleID,
                    CompanyName = s.Mobile!.Model!.Company!.CompanyName,
                    ModelName = s.Mobile!.Model!.ModelName,
                    s.IMEINo,
                    s.Price
                })
                .ToList();

                if (sales.Count == 0)
                {
                    return NotFound($"Không có đơn bán nào từ ngày {from.Date.Date} đến {to.Date.Date}.");
                }

                decimal totalPrice = sales.Sum(s => s.Price);
                return Ok(new
                {
                    sales,
                    TotalPriceInDate = totalPrice,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/sale/search/{imei}
        [HttpGet("search/{imei}")]
        public IActionResult GetSaleByIMEI(string imei)
        {
            try
            {
                var sale = _context.Sales
                    .Include(s => s.Customer)
                    .Where(s => s.IMEINo == imei)
                    .Select(s => new
                    {
                        CustomerName = s.Customer!.CustomerName,
                        MobileNumber = s.Customer!.MobileNo,
                        Email = s.Customer!.Email,
                        Address = s.Customer!.Address
                    })
                    .ToList();
                if (sale.Count == 0)
                {
                    return NotFound($"Không tìm thấy đơn hàng với IMEI: {imei}");
                }
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
