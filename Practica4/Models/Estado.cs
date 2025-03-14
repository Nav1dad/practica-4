using System.ComponentModel.DataAnnotations;

namespace Practica4.Models
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Estado")]

        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        [Display(Name = "Descripcion")]

        public string? Descripcion { get; set; }
        [StringLength(20)]
        [Display(Name = "Color")]

        public string? Color { get; set; }
        [Display(Name = "Creado en")]

        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Actualizado en")]

        public DateTime? UpdatedAt { get;set; }
    }
}
