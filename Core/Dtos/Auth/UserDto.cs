﻿using System.Text.Json.Serialization;

namespace Core.Dtos.Auth
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }
    }
}
