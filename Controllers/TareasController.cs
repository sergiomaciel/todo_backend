using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoCrudApiRestFull.Models;
using TodoCrudApiRestFull.ViewModels;

namespace TodoCrudApiRestFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareaContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TareasController(TareaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: api/Tareas
        //Acepta 3 filtros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas( int id = 0 , String descripcion = null, int estado = 0)
        {
            IQueryable<Tarea> query = _context.Tareas;

            if (id != 0)
                query = query.Where(d => d.id == id);
            if (!string.IsNullOrWhiteSpace(descripcion))
                query = query.Where(d => d.descripcion.Contains(descripcion));
            if (estado != 0)
                query = query.Where(d => d.estado == estado);

            List<Tarea> tareas = await query.ToListAsync();
            //List<Tarea> tareas = await _context.Tareas.ToListAsync();

            foreach (Tarea tarea in tareas)
            {
                //tarea.foto = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\images"}";
                //tarea.foto = Path.Combine(webHostEnvironment.WebRootPath, "images", tarea.foto);
                tarea.foto = "http://localhost/" + tarea.foto;
            }

            return tareas;
        }

        // GET: api/Tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound();
            }

            tarea.foto = "http://localhost/" + tarea.foto;
            return tarea;
        }

        // PUT: api/Tareas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<Tarea>> PutTarea(int id, [FromForm] Tarea tarea)
        {

            if (id != tarea.id)
            {
                return BadRequest();
            }

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return tarea;
        }

        //Se usa al se modifica la tarea y se debe cargar una nueva imagen en el server
        [HttpPut("{id}/img")]
        public async Task<ActionResult<Tarea>> PutImagenTarea(int id, [FromForm]  TareaViewModels model)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
            {
                return BadRequest();
            }

            string uniqueFileName = UploadedFile(model);

            tarea.descripcion = model.descripcion;
            tarea.estado = model.estado;
            tarea.foto = uniqueFileName;

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return tarea;
        }

        // POST: api/Tareas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea([FromForm] TareaViewModels model)
        {
            string uniqueFileName = UploadedFile(model);
            
            Tarea tarea = new Tarea
            {
                descripcion = model.descripcion,
                estado = model.estado,
                foto = uniqueFileName,
            };
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarea", new { id = tarea.id }, tarea);
        
        }

        // DELETE: api/Tareas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tarea>> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return tarea;
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.id == id);
        }

        private string UploadedFile(TareaViewModels archivo)
        {
            string uniqueFileName = null;

            if (archivo.foto != null)
            {
                string path = Path.Combine(webHostEnvironment.WebRootPath, "images");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.foto.FileName;
                string filePath = Path.Combine(path, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    archivo.foto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
