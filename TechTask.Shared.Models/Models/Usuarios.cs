using System.ComponentModel.DataAnnotations;

namespace TechTask.Models;

public class Usuarios
{
    public Usuarios() { }
    public Usuarios(string nome, string email, string? organizacao) 
    { 
        Nome = nome;
        Email = email;
        Organizacao = organizacao;
    }
    public int ID { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    public string? Organizacao { get; set; }

    public ICollection<Tarefas> Tarefas { get; set; }
}
