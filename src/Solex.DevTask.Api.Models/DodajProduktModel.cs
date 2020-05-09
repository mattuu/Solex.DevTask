using System;
using System.ComponentModel.DataAnnotations;

namespace Solex.DevTask.Api.Models
{
    public class DodajProduktModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "10000")]
        public decimal Ilosc { get; set; }
    }
}