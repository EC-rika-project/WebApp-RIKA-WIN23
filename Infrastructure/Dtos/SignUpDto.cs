﻿namespace Infrastructure.Dtos;

public class SignUpDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

}