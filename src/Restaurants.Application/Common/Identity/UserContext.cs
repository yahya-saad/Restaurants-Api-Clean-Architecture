﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Common.Identity;
public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null)
            throw new InvalidOperationException("User context is not present");


        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;


        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = user.FindFirst(ClaimTypes.Email)?.Value;

        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        var nationality = user.FindFirstValue("Nationality");
        var dateOfBirthString = user.FindFirstValue("DateOfBirth");

        var dateofBirth = dateOfBirthString != null
            ? DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd")
            : (DateOnly?)null;

        return new CurrentUser(userId, email, roles, nationality, dateofBirth);
    }
}
