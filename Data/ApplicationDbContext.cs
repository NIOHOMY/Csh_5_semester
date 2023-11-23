﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext :  IdentityDbContext<Microsoft.AspNetCore.Identity.IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}