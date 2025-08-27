using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.GET
{
    public class RecipeGet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Guid UploaderId { get; set; }
        public string UploaderName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
