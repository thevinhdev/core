﻿namespace AuthApi.Models
{
    public record User(string Username, string Password, string Role, string Tenant, string[] Scopes);
}
