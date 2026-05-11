using System;
using MarceloSilva.Models;
using Microsoft.EntityFrameworkCore;

namespace MarceloSilva.Data;

public class AppDataContext : DbContext
{
    public DbSet<Livro> Livros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ViniciusFranca_MarceloSilva.db");
    }
}
