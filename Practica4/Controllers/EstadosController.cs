using Microsoft.AspNetCore.Mvc;
using Practica4.Data;
using Practica4.Models;

namespace Practica4.Controllers
{
    public class EstadosController : Controller
    {
        private readonly AgendaDbContext _context;
        public EstadosController(AgendaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listaEstados = _context.Estados.ToList(); // SELECT * FROM Estados
            return View(listaEstados);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Estado estado)
        {
            if (ModelState.IsValid)
            {
                estado.CreatedAt = DateTime.Now;

                _context.Estados.Add(estado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estado);
        }
    }
}
