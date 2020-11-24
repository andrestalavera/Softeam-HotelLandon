using Azure.Storage;
using Azure.Storage.Blobs;
using HotelLandon.Data;
using HotelLandon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLandon.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HotelLandonContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(HotelLandonContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CustomersController>();
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Welcome to Customers page");
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation($"Welcome to Customer page, id {id}");
            if (id == null)
            {
                _logger.LogError("Id is null");
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                _logger.LogError($"Customer not found");
                return NotFound();
            }
            _logger.LogInformation($"{customer.FirstName} {customer.LastName} found. Returning to details");

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,BirthDate,Id")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,BirthDate,Id")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export()
        {
            var customers = await _context.Customers.ToListAsync();
            foreach (var customer in customers)
            {
                // Newtonsoft.Json
                var json = JsonConvert.SerializeObject(customer);
                // send to Azure Storage

                var config = new
                {
                    AccountName = "softeamandrestalavera001",
                    ImageContainer = "demo001",
                    AccountKey = "T3ZmtK4kCoUUKWdiMTb2zYSHaWifC8Ikd4lqAFr487YpFxNLxCfTl5GCaLPJP8pIlNGOtcOcUnkx87JwAyhwyw=="
                };

                byte[] byteArray = Encoding.ASCII.GetBytes(json);
                using MemoryStream stream = new MemoryStream(byteArray);
                var blobUri = new Uri("https://" +
                              config.AccountName +
                              ".blob.core.windows.net/" +
                              config.ImageContainer +
                              "/" + Guid.NewGuid());

                var storageCredentials = new StorageSharedKeyCredential(config.AccountName, config.AccountKey);
                var blobClient = new BlobClient(blobUri, storageCredentials);

                await blobClient.UploadAsync(stream, overwrite: true);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
