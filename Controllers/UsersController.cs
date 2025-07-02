using BackEnd_MobileShop.Models;
using BackEnd_MobileShop.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace BackEnd_MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MobileShopDbContext _context;
        public UsersController(MobileShopDbContext context)
        {
            _context = context;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            try
            {
                // Kiểm tra user đã tồn tại chưa
                if (_context.Users.Any(u => u.UserName == req.UserName))
                {
                    return BadRequest("UserName đã tồn tại.");
                }
                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    UserName = req.UserName,
                    FullName = req.FullName,
                    Address = req.Address,
                    Phone = req.Phone,
                    Role = "Employee"
                };
                user.Password = hasher.HashPassword(user, req.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("Đăng ký thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đăng ký thất bại: {ex.Message}");
            }
            
        }

        // POST: api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == req.UserName);
                if (user == null)
                    return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.Password!, req.Password);

                if (result == PasswordVerificationResult.Failed)
                    return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    user.UserName,
                    user.FullName,
                    user.Role
                });
            }
            catch (Exception ex) {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
            
        }
        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                return Ok(users);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Lỗi khi lấy danh sách user: {ex.Message}");
            }
        }
        // GET: api/users/{username}
        [HttpGet("{username}")]
        public IActionResult GetUserByUserName(string username)
        {
            try
            {
                var user = _context.Users
                    .Where(u => u.UserName == username)
                    .Select(u => new
                    {
                        u.UserName,
                        u.FullName,
                        u.Address,
                        u.Phone,
                        u.Role
                    })
                    .FirstOrDefault();
                if (user == null)
                {
                    return NotFound($"Không tìm thấy user với tên đăng nhập: {username}");
                }
                return Ok(user);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy user: {ex.Message}");
            }
        }

        // PUT: api/users/{username}
        [HttpPut("{username}")]
        public IActionResult UpdateUser(string username, [FromBody] User updatedUser)
        {
            try 
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.UserName == username);
                if (existingUser == null)
                {
                    return NotFound($"Không tìm thấy user: {username}");
                }

                existingUser.FullName = updatedUser.FullName;
                existingUser.Address = updatedUser.Address;
                existingUser.Phone = updatedUser.Phone;
                existingUser.Role = updatedUser.Role;

                _context.SaveChanges();
                return Ok("Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật user: {ex.Message}");
            }
        }

        // DELETE: api/users/{username}
        [HttpDelete("{username}")]
        public IActionResult DeleteUser(string username)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    return NotFound($"Employee {username} không tồn tại!");
                }
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Ok("Xoá user thành công");

            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa user: {ex.Message}");
            }
        }

        // POST: api/users/
        [HttpPost]
        public IActionResult Add([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }
            try
            {
                // Kiểm tra user đã tồn tại chưa
                if (_context.Users.Any(u => u.UserName == req.UserName))
                {
                    return BadRequest("UserName đã tồn tại.");
                }
                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    UserName = req.UserName,
                    FullName = req.FullName,
                    Address = req.Address,
                    Phone = req.Phone,
                    Role = "Employee"
                };
                user.Password = hasher.HashPassword(user, req.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok("Thêm Employee thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Thêm Employee thất bại: {ex.Message}");
            }

        }
    }

}
