using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RecipieBox.Models
{
  public class RecipieBoxContextFactory : IDesignTimeDbContextFactory<RecipieBoxContext>
  {
    RecipieBoxContext IDesignTimeDbContextFactory<RecipieBoxContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var builder = new DbContextOptionsBuilder<RecipieBoxContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]));

      return new RecipieBoxContext(builder.Options);
    }
  }
}