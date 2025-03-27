using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practica4.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Evento")]

        public string Title { get; set; } = string.Empty;
        [StringLength(255)]
        [Display(Name = "Descripcion")]

        public string? Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha de inicio")]

        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Fecha fin")]
        public DateTime EndDate { get; set; }

        [Display(Name ="Estado")]
        [ForeignKey("EstadoId")]

        public int EstadoId { get; set; }

        public Estado? Estado {  get; set; }

        [Display(Name = "Creado en")]

        public DateTime? CreatedAd { get; set; }
        [Display(Name = "Actualizado en")]

        public DateTime? UpdatedAt { get; set; }
    }
}
