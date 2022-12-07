﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.EntryDbModels.Order;

public class OrderDetails
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }


    [Required]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    public int Unit { get; set; }
    public double PricePerUnit { get; set; }
}