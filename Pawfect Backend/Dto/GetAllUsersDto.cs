﻿namespace Pawfect_Backend.Dto
{
    public class GetAllUsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool isBlocked { get; set; }
    }
}
