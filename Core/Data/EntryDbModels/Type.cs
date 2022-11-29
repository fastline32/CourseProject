using System.ComponentModel.DataAnnotations;

namespace Core.Data.EntryDbModels
{
    public class Type
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}
