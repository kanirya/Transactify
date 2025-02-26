
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hostel_Management.Models
{
  
        public class Category
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            // Navigation property (One category can have many products)
            public ICollection<Product> Products { get; set; }
        
    }
}

