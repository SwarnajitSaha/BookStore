using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product ProductObj { get; set; }

        [Range(0, 1000, ErrorMessage = "Please Enter value between 1 to 1000")]
        public int Count { get; set; }

        public string ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [ValidateNever]
        public ApplicationUser ApplicationUser {get; set; }

        [NotMapped] // this will ensure not to save this property in DB but we can use it UI;
        public double Price { get; set; }

    }
}
