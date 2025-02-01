﻿using System.ComponentModel.DataAnnotations;

namespace LibrosApi.Dto
{
    public class GetUserDto
    {
        public int Id { get; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
