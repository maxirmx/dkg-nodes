﻿// Copyright (C) 2024 Maxim [maxirmx] Samsonov (www.sw.consulting)
// All rights reserved.
// This file is a part of dkg service node
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
// notice, this list of conditions and the following disclaimer in the
// documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using dkgServiceNode.Data;
using dkgServiceNode.Models;
using dkgServiceNode.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dkgServiceNode.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IJwtUtils _jwtUtils;


        public AuthController(UserContext context, IJwtUtils jwtUtils)
        {
            _context = context;
            _jwtUtils = jwtUtils;
        }

        // POST: api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserViewItem>> Login(Credentials crd)
        {
            User? user = await _context.Users.Where(u => u.Email == crd.Email).SingleOrDefaultAsync();
            if (user == null) return Unauthorized(new { message = "Неправильный адрес электронной почты или пароль" });
            if (!BCrypt.Net.BCrypt.Verify(crd.Password, user.Password)) return Unauthorized();

            UserViewItemWithJWT userViewItem = new(user)
            {
                Token = _jwtUtils.GenerateJwtToken(user)
            };
            return userViewItem;
        }

        // GET: api/auth/check
        [HttpGet("check")]
        public IActionResult Check()
        {
            return NoContent();
        }

        // dummy method to test the connection
        [HttpGet("hello")]
        [AllowAnonymous]
        public ActionResult Test()
        {
            return Ok(new { message = "Hello, world!" });
        }

    }
    }
