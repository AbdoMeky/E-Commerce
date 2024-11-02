using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.CategoryDTO
{
    public class ShowCategoryDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ShowCategoryDTO()
        {
            
        }
        public ShowCategoryDTO(Category category)
        {
            this.Name = category.Name;
            this.Description = category.Description;
        }
    }
}
