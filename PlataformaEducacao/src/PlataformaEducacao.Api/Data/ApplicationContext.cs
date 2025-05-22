using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PlataformaEducacao.Api.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext(options)
{
}