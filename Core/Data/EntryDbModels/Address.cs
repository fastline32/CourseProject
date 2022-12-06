using System.ComponentModel.DataAnnotations;

namespace Core.Data.EntryDbModels;

public class Address
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string City { get; set; } = null!;

    [Required] 
    public string StreetAddress { get; set; } = null!;

    [Required] 
    public string ZipCode { get; set; } = null!;
}