using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using TechTask.Data;
using TechTask.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TechTask.Request;
namespace TechTask.EndPoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this IEndpointRouteBuilder routes)
    {
        //endpoint para fazer o get de todas as tarefas do db, não utilizado no frontend mas como padrão deixei feito para fins de testes
        var group = routes.MapGroup("/api/Tarefas").WithTags(nameof(Tarefas));

        group.MapGet("/", async (TechTaskContext db) =>
        {
            return await db.Tarefas.ToListAsync();
        })
        .WithName("RetornaTodasAsTarefas")
        .WithOpenApi();

        //endpoint utilizado para fazer o get pelo id da tarefam onde ele percorre todo o db entra a primeira tarefa com id igual e se não nula retorna a tarefa(função utilizada na atualização da tarefa pelo frontend)
        group.MapGet("/por-id/{id}", async Task<Results<Ok<Tarefas>, NotFound>> (int id, TechTaskContext db) =>
        {
            var tarefa = await db.Tarefas.AsNoTracking()
            .FirstOrDefaultAsync(t => t.ID == id);

            return tarefa is not null
            ? TypedResults.Ok(tarefa)
            : TypedResults.NotFound();
        })
        .WithName("RetornaTarefaPorId")
        .WithOpenApi();

        //endpoint utilizado para fazer o get de todas as tarefas que utilizam a chave estrangeira/idUsuario, utilizada para retornar todas as tarefas de um id de usuario especifico.
        group.MapGet("/{idUsuario}", async Task<Results<Ok<List<Tarefas>>, NotFound>> (int idUsuario, TechTaskContext db) =>
        {
            var tarefas = await db.Tarefas.AsNoTracking()
            .Where(t => t.IDUsuario == idUsuario)
            .ToListAsync();

            return tarefas.Any()
            ? TypedResults.Ok(tarefas)
            : TypedResults.NotFound();
        })
        .WithName("RetornaTarefasDoUsuario")
        .WithOpenApi();
        //endpoint utilizado para fazer o put e a atualização de diversos dados da tarefa.
        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Tarefas tarefa, TechTaskContext db) =>
        {
            var affected = await db.Tarefas
                .Where(model => model.ID == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Tarefa, tarefa.Tarefa)
                    .SetProperty(m => m.Descricao, tarefa.Descricao)
                    .SetProperty(m => m.Prazo, tarefa.Prazo)
                    .SetProperty(m => m.Urgencia, tarefa.Urgencia)
                    .SetProperty(m => m.Estado, tarefa.Estado)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("AtualizaTarefa")
        .WithOpenApi();

        //endpoint utilizado para fazer o put apenas do estado da tarefa, facilitando a inplementação dos botões que atualizam somente o estado(sendo bem redundante)
        group.MapPut("/{id}/estado", async Task<Results<Ok, NotFound>> (int id, string estado, TechTaskContext db) =>
        {
            var affected = await db.Tarefas
                .Where(model => model.ID == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Estado, estado)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("AtualizaEstadoDaTarefa")
        .WithOpenApi();

        //endpoint de post utilizado para fazer a criação e o cadastro da nova tarefa no db.
        group.MapPost("/", async (TechTaskContext db,[FromBody] TarefaRequest tarefaRequest) =>
        {
            var tarefa = new Tarefas(tarefaRequest.tarefa, tarefaRequest.descricao, tarefaRequest.urgencia, tarefaRequest.insercao, tarefaRequest.prazo,  tarefaRequest.estado, tarefaRequest.idUsuario);
            db.Tarefas.Add(tarefa);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Tarefas/{tarefa.ID}", tarefa);
        })
        .WithName("CriaTarefa")
        .WithOpenApi();

        //endpoint para fazer o delete de uma tarefa pelo id, não utilizado no projeto, eu particularmente não gosto de excluir em definitivo arquivo do db, prefiro inativalos.
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, TechTaskContext db) =>
        {
            var affected = await db.Tarefas
                .Where(model => model.ID == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletaTarefa")
        .WithOpenApi();
    }
}