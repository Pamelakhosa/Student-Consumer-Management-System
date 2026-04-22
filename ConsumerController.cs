
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETCore_DB.Data;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using ASPNETCore_DB.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ASPNETCore_DB.Repositories;

namespace ASPNETCore_DB.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ConsumerController : Controller
    {
        private readonly SQLiteDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConsumer _consumerRepo;

        public ConsumerController(SQLiteDBContext context, IConsumer consumerRepo, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                _context = context;
                _consumerRepo = consumerRepo;
                _webHostEnvironment = webHostEnvironment;
                _httpContextAccessor = httpContextAccessor;
            }
            catch (Exception ex) 
            {
                throw new Exception("Constructor not initialized - IConsumer consumerRepo ");
            }
        }

        // GET: Consumer
        [Authorize(Roles = "Admin")]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 3;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["StudentNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            ViewResult viewResult = View();

            try
            {
                viewResult = View(PaginatedList<Consumer>.Create(_consumerRepo.GetConsumers(searchString, sortOrder).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                throw new Exception("No student records detected");
            }

            return viewResult;
        }
        /*public async Task<IActionResult> Index()
        {
            var consumers = _context.Consumers.ToList(); // or wherever your context is injected
            return View(consumers);
        }*/

        // GET: Consumer/Details/5
        public IActionResult Details(string id)
        {
            /*if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers
                .FirstOrDefaultAsync(m => m.ConsumerId == id);
            if (consumer == null)
            {
                return NotFound();
            }

            return View(consumer);*/

            if (string.IsNullOrEmpty(id))
            {
                var consumer = _consumerRepo.GetByEmail(this.User.Identity.Name.ToString());
                return View(consumer);
            }
            else 
            {
                var consumer = _consumerRepo.Details(id);
                return View(consumer);
            }
        }

        // GET: Consumer/Create
        [Authorize(Roles = "Consumer")]
        [HttpGet]
        public IActionResult Create() 
        {
            var consumerExist = _consumerRepo.GetByEmail(this.User.Identity.Name.ToString());
            if (consumerExist != null)
            {
                return RedirectToAction("Details", "Consumer", consumerExist.Email);
            }
            else 
            {
                Consumer consumer = new Consumer();
                string fileName = "DefaultPic.png";
                consumer.Photo = fileName;
                return View(consumer);
            }
        }
        /* public IActionResult Create()
         {
             return View();
         } */

        // POST: Consumer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Consumer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Consumer consumer)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WebConstants.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),
            FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }
            consumer.Photo = fileName + extension;
            try
            {
                if (ModelState.IsValid)
                {
                    _consumerRepo.Create(consumer);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("Consumer record no saved");
            }
            return View(consumer);
        }

        /* public async Task<IActionResult> Create(Consumer consumer, IFormFile PhotoFile)
         {
             if (ModelState.IsValid)
             {
                 if (PhotoFile != null && PhotoFile.Length > 0)
                 {
                     var fileName = Path.GetFileName(PhotoFile.FileName);
                     var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         await PhotoFile.CopyToAsync(stream);
                     }

                     consumer.Photo = "/images/" + fileName;
                 }

                 _context.Add(consumer);
                 await _context.SaveChangesAsync();
                 return RedirectToAction("Create");
             }
             return View(consumer);
         } */


        // GET: Consumer/Edit/5

        [Authorize(Roles = "Consumer")]
        [HttpGet]
        public IActionResult Edit(string id) 
        {
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_consumerRepo.Details(id));
            }
            catch (Exception ex) 
            {
                throw new Exception("Consumer details not found");
            }
            return viewDetail;
        }
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return NotFound();
            }
            return View(consumer);
        }*/


        // POST: Consumer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Consumer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Consumer consumer)
        {
            string photoName = Request.Form["PhotoName"].ToString();
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + WebConstants.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                var oldFile = Path.Combine(upload, photoName);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),
                FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                consumer.Photo = fileName + extension;
            }
            else
            {
                consumer.Photo = photoName;
            }
            try
            {
                _consumerRepo.Edit(consumer);
            }
            catch (Exception ex)
            {
                throw new Exception("Student record not saved.");
            }
            return RedirectToAction("Index");
        }
        /*public async Task<IActionResult> Edit(int id, [Bind("ConsumerId,FullName,ContactNumber,Email,Photo")] Consumer consumer)
        {
            if (id != consumer.ConsumerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumerExists(consumer.ConsumerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(consumer);
        }*/

        // GET: Consumer/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(string id) 
        {
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_consumerRepo.Details(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Consumer details not found");
            }
            return viewDetail;
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers
                .FirstOrDefaultAsync(m => m.ConsumerId == id);
            if (consumer == null)
            {
                return NotFound();
            }

            return View(consumer);
        }
        
        // POST: Consumer/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("FirstName, Surname, Email")] Consumer consumer)
        {
            try
            {
                _consumerRepo.Delete(consumer);
            }
            catch (Exception ex)
            {
                throw new Exception("Consumer could not be deleted");
            }

            return RedirectToAction(nameof(Index));
        }
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             if (_context.Consumers == null)
             {
                 return Problem("Entity set 'SQLiteDBContext.Consumers'  is null.");
             }
             var consumer = await _context.Consumers.FindAsync(id);
             if (consumer != null)
             {
                 _context.Consumers.Remove(consumer);
             }

             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
         }

        private bool ConsumerExists(int id)
        {
          return _context.Consumers.Any(e => e.ConsumerId == id);
        }
    }//end class 
}//end namespace
