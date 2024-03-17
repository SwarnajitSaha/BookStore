using BookStore.Models.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderHaderId { get; set; }
        [ForeignKey("OrderHaderId")]
        [ValidateNever]
        public OrderHeader OrderHeader_Nev { get; set; } //this is for navigate the base table

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Prodct_Nev { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

    }
}
