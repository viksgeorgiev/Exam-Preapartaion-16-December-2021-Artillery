namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Shell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } 
            = new HashSet<Gun>();
    }
}
