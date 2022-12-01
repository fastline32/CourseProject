using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.EntryDbModels
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
