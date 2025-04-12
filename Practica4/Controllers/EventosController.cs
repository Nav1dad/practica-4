using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica4.Data;
using Practica4.Models;

namespace Practica4.Controllers
{
    public class EventosController : Controller
    {
        private readonly AgendaDbContext _context;
        private const int PageSize = 5; // paginas maximas

        public EventosController(AgendaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, int page = 1)
        {
            var query = _context.Eventos.AsQueryable();

            // aplicar filtro de busqueda

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Title.Contains(search) || e.Descripcion.Contains(search) || e.Estado.Name.Contains(search));
            }


            // aplicar paginacion
            var listaEventos = query
                .OrderBy(e => e.Id)
                .Include(e => e.Estado)
                .Skip((page - 1) * PageSize) // Saltar los registros de las paginas anteriores
                .Take(PageSize)
                .ToList();

            // Contara numero de registros
            var totalRegistros = query.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPaginas;
            ViewBag.Search = search;

            return View(listaEventos);
        }


        public IActionResult Create()
        {
            var estados = _context.Estados.ToList(); // Select * form Estados

            ViewData["EstadoId"] = new SelectList(estados, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.CreatedAd = DateTime.Now;

                _context.Eventos.Add(evento);
                _context.SaveChanges();

                //Mensaje de exito
                TempData["Mensaje"] = "El evento se ha creado corretamente";
                TempData["MessageType"] = "success";

                return RedirectToAction("Index");
            }

            //Mensaje de error
            TempData["Message"] = "Erro al crear el evento. Verifica los datos";
            TempData["MessageType"] = "error";

            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Name", evento.EstadoId);

            return View(evento);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var evento = _context.Eventos.Include(e => e.Estado).FirstOrDefault(e => e.Id == id); // select * from eventos join estados on eventos.estadoId = EstadosId 
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var evento = _context.Eventos.Find(id); // select * from Eventos where Id = id
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Name", evento.EstadoId);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            var current = _context.Eventos.Find(id); // select * from Eventos where ID = id

            if (current == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                current.Title = evento.Title;
                current.Descripcion = evento.Descripcion;
                current.StartDate = evento.StartDate;
                current.EndDate = evento.EndDate;
                current.EstadoId = evento.EstadoId;
                current.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                //Mesnaje de Exito
                TempData["Mensaje"] = "El evento se ha actualizado corretamente";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }

            //Mensaje de error
            TempData["Message"] = "Error al actualizar el evento. Verifica los datos";
            TempData["MessageType"] = "error";

            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Name", evento.EstadoId);

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }
            _context.Eventos.Remove(evento);
            _context.SaveChanges();
            // Mensaje de exito
            TempData["Mensaje"] = "El evento se ha eliminado corretamente";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> GetEventos()
        {
            var eventos = await _context.Eventos.Include(e => e.Estado).Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartDate,
                end = e.EndDate,
                description = e.Descripcion,
                Color = e.Estado.Color
            }).ToListAsync();
            return Json(eventos);
        }
    }
}