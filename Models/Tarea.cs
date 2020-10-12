using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoCrudApiRestFull.Models
{
    public class Tarea
    {
        [Key]
        public int id { get; set; }
        //[Required]
        public String descripcion { get; set; }
        //[Required]
        public int estado { get; set; }
        //[Required]
        public string foto { get; set; }
    }
}
