using MarceloSilva.Data;
using MarceloSilva.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

app.MapGet("/api/livro/buscar/{nome}", (string nome, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.FirstOrDefault(l => l.Nome == nome);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado!");
    }

    return Results.Ok(livro);
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

app.MapPut("/api/livro/emprestar/{nome}", (string nome, Livro livroAtualizado, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.FirstOrDefault(s => s.Nome == nome);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado!");
    }

    if (livro.Emprestado == true)
    {
        return Results.Conflict("Livro emprestado anteriormente!");
    }

    livro.Emprestado = livroAtualizado.Emprestado;
    ctx.SaveChanges();

    return Results.Ok(livro);
});

app.MapPut("/api/livro/devolver/{nome}", (string nome, Livro livroAtualizado, [FromServices] AppDataContext ctx) =>
{
    var livro = ctx.Livros.FirstOrDefault(s => s.Nome == nome);

    if (livro == null)
    {
        return Results.NotFound("Livro não encontrado!");
    }

    if (livro.Emprestado == false)
    {
        return Results.Conflict("Livro não foi emprestado anteriormente!");
    }

    livro.Emprestado = livroAtualizado.Emprestado;
    ctx.SaveChanges();

    return Results.Ok(livro);
});

app.MapGet("/api/livro/disponiveis", ([FromServices] AppDataContext ctx) =>
{
    var resultado = ctx.Livros.ToList();
    
    if (resultado == null)
    {
        return Results.NotFound("Nenhum livro encontrado!");
    }
    
    List<Livro> livrosDisponiveis = new List<Livro>();
    for(int i = 0; i < resultado.Count; i++)
    {
        if(resultado[i].Emprestado == false)
        {
            livrosDisponiveis.Add(resultado[i]);
        }
    }

    return Results.Ok(livrosDisponiveis.ToList());
});

app.MapGet("/api/livro/emprestados", ([FromServices] AppDataContext ctx) =>
{
    var resultado = ctx.Livros.ToList();
    
    if (resultado == null)
    {
        return Results.NotFound("Nenhum livro encontrado!");
    }
    
    List<Livro> livrosDisponiveis = new List<Livro>();
    for(int i = 0; i < resultado.Count; i++)
    {
        if(resultado[i].Emprestado == true)
        {
            livrosDisponiveis.Add(resultado[i]);
        }
    }

    return Results.Ok(livrosDisponiveis.ToList());
});

app.Run();
