using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.POST
{
    public class UserCommentPost
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
        public string Message { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
