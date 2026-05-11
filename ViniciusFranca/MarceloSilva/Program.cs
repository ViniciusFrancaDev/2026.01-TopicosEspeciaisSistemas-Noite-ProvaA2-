using MarceloSilva.Data;
using MarceloSilva.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

app.MapGet("/api/livro/listar", ([FromServices] AppDataContext ctx) =>
{
    var resultado = ctx.Livros.FirstOrDefault();

    if (resultado == null)
    {
        return Results.NotFound("Nenhum livro encontrado!");
    }

    return Results.Ok(ctx.Livros.ToList());
});

app.MapPost("/api/livro/cadastrar", ([FromBody] Livro livro, [FromServices] AppDataContext ctx) =>
{
    var resultado = ctx.Livros.FirstOrDefault(l => l.Nome == livro.Nome);

    if (resultado != null)
    {
        return Results.Conflict("Esse livro ja existe!");
    }

    ctx.Livros.Add(livro);
    ctx.SaveChanges();

    return Results.Created("", livro);
});

app.Run();
