﻿namespace Infrastructure.Dtos;

public class SignInDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
    public string ExternalProvider { get; set; }
    public string ExternalProviderId { get; set; }
}
