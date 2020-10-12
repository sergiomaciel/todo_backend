using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using TodoCrudApiRestFull.Models;

namespace TodoCrudApiRestFull.ViewModels
{
    public class TareaViewModels
    {
        [Key]
        public int id { get; set; }
        //[Required]
        public String descripcion { get; set; }
        //[Required]
        public int estado { get; set; }
        //[Required]
        public IFormFile foto { get; set; }
    }
}
