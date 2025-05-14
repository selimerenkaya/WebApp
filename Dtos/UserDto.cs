//DTO, API'ye gelen ve giden veriyi tanımlayan özel sınıflardır. 
//Swaggerdan API’ye gelen isteklerde veya dönen cevaplarda kullanılan veri yapılarıdır.

//datayı object e cevirme kısmı
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatForLife.Dtos
{
    // API'ye gelen ve giden veriyi temsil eden veri taşıyıcı sınıf
    public class UserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
