﻿using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser()
        {
            FirstName = registerDto.FirstName ,
            LastName = registerDto.LastName ,
            Email = registerDto.Email ,
            UserName = registerDto.Email
        };

        var result = await signInManager.UserManager
            .CreateAsync(user , registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code , error.Description);
            }

            return ValidationProblem();
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false)
            return NoContent();

        var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

        return Ok(new
        {
            user.FirstName ,
            user.LastName ,
            user.Email ,
            Address = user.Address?.ToDto() ,
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("auth-status")]
    public IActionResult GetAuthState()
    {
        return Ok(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<IActionResult> CreateOrUpdateAddress(
        AddressDto addressDto
    )
    {
        var user = await signInManager.UserManager
            .GetUserByEmailWithAddress(User);

        if (user.Address is null)
        {
            user.Address = addressDto.ToEntity();
        }
        else
        {
            user.Address.UpdateFromDto(addressDto);
        }

        var result = await signInManager.UserManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest("Problem Updating User Address");
        }

        return Ok(user.Address.ToDto());
    }


}
