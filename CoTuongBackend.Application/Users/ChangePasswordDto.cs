﻿namespace CoTuongBackend.Application.Users;

public sealed record ChagePasswordDto
{
    public required string UserNameOrEmail { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string OldPassword { get; set; }
}