using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookStore.Models.Model
{
    public class Product
    {
        [Key] //data anotation for database scema
        public int Id { get; set; }

        [Required]

        public string Title { get; set; }

        public string Description { get; set; }
        [Required]

        public string ISBN { get; set; }
        [Required]

        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        //for foreign key

        public int CategoryId { get; set; } //to define this porperyu as foreign key we need to add two line
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; } //nevigation property
        [ValidateNever]
        public string ImageUrl {  get; set; }
    }
}
