﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.API.DTO.POST
{
    public class UserPost
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
