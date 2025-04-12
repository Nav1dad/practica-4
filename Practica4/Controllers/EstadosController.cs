using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica4.Data;
using Practica4.Models;

namespace Practica4.Controllers
{
    public class EstadosController : Controller
    {
        private readonly AgendaDbContext _context;
        private const int PageSize = 5; // paginas maximas
        public EstadosController(AgendaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string search, int page = 1)
        {
            var query = _context.Estados.AsQueryable(); // Trabajando con Estados

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Descripcion.Contains(search));
            }

            // Aplicar paginación
            var listaEstados = query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Contar el número de registros
            var totalRegistros = query.Count();
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPaginas;
            ViewBag.Search = search;

            return View(listaEstados); // Pasar lista de estados a la vista
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

                //Mensaje de exito
                TempData["Mensaje"] = "El estado se ha creado corretamente";
                TempData["MessageType"] = "success";

                return RedirectToAction("Index");
            }

            //Mensaje de error
            TempData["Message"] = "Erro al crear el estado. Verifica los datos";
            TempData["MessageType"] = "error";

            return View(estado);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var estado = _context.Estados.Find(id);

            if (estado == null)
            {
                return NotFound();
            }
            return View(estado);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var estado = _context.Estados.Find(id);

            if (estado == null)
            {
                return NotFound();
            }
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(int id, Estado estado)
        {
            if (id != estado.Id)
            {
                return NotFound();
            }

            var current = _context.Estados.Find(id);

            if (current == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                current.Name = estado.Name;
                current.Descripcion = estado.Descripcion;
                current.Color = estado.Color;
                current.UpdatedAt = DateTime.Now;

                _context.Update(current);
                _context.SaveChanges();

                //Mensaje de exito
                TempData["Message"] = "El estado se ha actualizado correctamente";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }

            //Mensaje de error
            TempData["Message"] = "Error al actualizar el estado. Verifica los datos";
            TempData["MessageType"] = "error";
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            var estado = _context.Estados.Find(id);

            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            _context.SaveChanges();

            //Mensaje de exito
            TempData["Message"] = "El estado se ha eliminado correctamente";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}