using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using TechTask.Data;
using TechTask.Models;
using System.ComponentModel.DataAnnotations;
using TechTask.Request;
using Microsoft.AspNetCore.Mvc;
namespace TechTask.EndPoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuarios").WithTags(nameof(Usuarios));

        //endpoint utilizado para fazer o get de todos os usuarios cadastrados
        group.MapGet("/", async (TechTaskContext db) =>
        {
            return await db.Usuarios.ToListAsync();
        })
        .WithName("RetornaTodosOsUsuarios")
        .WithOpenApi();

        //endpoint utilizado para fazer o get de um usuario pelo email cadastrado
        group.MapGet("/{email}", async Task<Results<Ok<Usuarios>, NotFound>> (string email, TechTaskContext db) =>
        {
            return await db.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Email == email)
                is Usuarios model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("RetornaUsuarioPorEmail")
        .WithOpenApi();

        //endpoint utilizado para fazer o get do usuario através do id, utilizado nas funções onde o frontend recupera o id do usuario dos cookies ou do useParams e retorna o usuario
        group.MapGet("/por-id/{id}", async Task<Results<Ok<Usuarios>, NotFound>> (int id, TechTaskContext db) =>
        {
            return await db.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ID == id)
                is Usuarios model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("RetornaUsuarioPorId")
        .WithOpenApi();

        //endpoint para fazer o put e atualizar algum dado que necessário do usuario
        group.MapPut("/{email}", async Task<Results<Ok, NotFound>> (string email, Usuarios usuarios, TechTaskContext db) =>
        {
            var affected = await db.Usuarios
                .Where(model => model.Email == email)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Nome, usuarios.Nome)
                    .SetProperty(m => m.Organizacao, usuarios.Organizacao)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("AtualizaUsuario")
        .WithOpenApi();


        //endpoint utilizado para fazer o post, o cadastro do usuario, não passei o id através dessa função deixando que ele seja gerado pelo backend, criei uma classe usuarioRequest onde ele passa apenas as informações que foram determinadas.
        group.MapPost("/", async ( TechTaskContext db, [FromBody] UsuariosRequest usuariosRequest ) =>
        {
            var usuario = new Usuarios(usuariosRequest.nome, usuariosRequest.email, usuariosRequest.organizacao);
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Usuarios/{usuario.ID}", usuario);
        })
        .WithName("CriaUsuario")
        .WithOpenApi();


        //endpoint utilizado para fazer o delete, como o de tarefas, não foi utilizado no projeto.
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, TechTaskContext db) =>
        {
            var affected = await db.Usuarios
                .Where(model => model.ID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletaUsuarios")
        .WithOpenApi();
    }
}
