﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp.API.Models;

namespace RecipeApp.API.DTO.GET
{
    public class RatingGet
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
    }
}
