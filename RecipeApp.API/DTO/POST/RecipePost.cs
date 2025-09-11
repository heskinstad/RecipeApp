using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class RecipePost
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UploaderId { get; set; }
        public float AvgRating { get; set; }
        public int Visits { get; set; }
    }
}
