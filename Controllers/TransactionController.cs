using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd_MobileShop.Models;

namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public TransactionController(MobileShopDbContext context)
        {
            _context = context;
        }

        // GET: api/transaction
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var transactions = _context.Transactions
                    .Include(t => t.Model)
                    .Select(t => new {
                        t.TransID,
                        t.ModelID,
                        t.Model!.ModelName,
                        t.Quantity,
                        t.Amount,
                        t.TransDate
                    })
                    .ToList();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách giao dịch: {ex.Message}");
            }
        }

        // POST: api/transaction
        [HttpPost]
        public IActionResult Create([FromBody] Transaction transaction)
        {
            try
            {
                transaction.TransDate = transaction.TransDate == default
                    ? DateTime.Now
                    : transaction.TransDate;
                var model = _context.Models.Find(transaction.ModelID);
                if (model != null)
                {
                    model.AvailableQty += transaction.Quantity;
                }

                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                return Ok($"Thêm giao dịch nhập kho thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi thêm giao dịch: {ex.Message}");
            }
        }

        // GET: api/transaction/nextid
        [HttpGet("nextid")]
        public IActionResult GetNextTransactionId()
        {
            try
            {
                int nextId = _context.Transactions.Any()
                ? _context.Transactions.Max(t => t.TransID) + 1
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
