using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.Data;
using Parser.Models;
using System.Text.Encodings.Web;

namespace Parser.Controllers
{
    public class UsersController : Controller
    {
        private readonly ParserContext _context;
        private IWebHostEnvironment hostEnv;
        private static string? indexContext;

        public UsersController(ParserContext context, IWebHostEnvironment env)
        {
            _context = context;
            hostEnv = env;
        }

        // GET: Users
        public async Task<IActionResult> Index(SortState sortOrder = SortState.NameAcs)
        {
            if (indexContext != null)
            {
                ViewData["error"] = indexContext;
                indexContext = null;
                return View();
            }

            //сортировка
            IQueryable<User> users = _context.User;
            ViewData["NameSort"] = sortOrder == SortState.NameAcs ? SortState.NameDesc : SortState.NameAcs;
            ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;

            users = sortOrder switch
            {
                SortState.NameDesc => users.OrderByDescending(s => s.FirstName),
                SortState.DateAsc => users.OrderBy(s => s.DateOfBirth),
                SortState.DateDesc => users.OrderByDescending(s => s.DateOfBirth),
                _ => users.OrderBy(s => s.FirstName)

            };

            return View(await users.AsNoTracking().ToListAsync());

        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                //парсинг csv
                var fileDic = "Files";
                string filePath = Path.Combine(hostEnv.WebRootPath, fileDic);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                var fileName = file.FileName;
                filePath = Path.Combine(filePath, fileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                }

                //конвертация в json
                Workbook book = new Workbook(filePath);
                string jsonFilePath = filePath.Replace(".csv", ".json");
                book.Save(jsonFilePath, SaveFormat.Json);

                //данные json
                StreamReader sr = new StreamReader(jsonFilePath);
                var data = sr.ReadToEnd();
                sr.Close();

                //свертка в лист user'ов
                var Users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(data);

                if (Users != null) foreach (var item in Users) await Create(item);


                return RedirectToAction("index");
            }
            catch
            {
                indexContext = "error";
                return RedirectToAction("index") ; 
            }

        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Index,UserId,FirstName,LastName,Sex,Email,Phone,DateOfBirth,JobTitle")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Index,UserId,FirstName,LastName,Sex,Email,Phone,DateOfBirth,JobTitle")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'ParserContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
