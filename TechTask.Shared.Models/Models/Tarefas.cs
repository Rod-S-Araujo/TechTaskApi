namespace TechTask.Models;

public
    class Tarefas
{
    public Tarefas()
    {

    } 
    public Tarefas(string tarefa, string descricao, string urgencia, DateTime? insercao, DateTime prazo, string estado, int idUsuario )
    {
        Tarefa = tarefa;
        Descricao = descricao;
        Urgencia = urgencia;
        Insercao = insercao;
        Prazo = prazo;
        Estado = estado;
        IDUsuario = idUsuario;  
    }


    public int ID { get; set; }
    public string Tarefa { get; set; }
    public string? Descricao { get; set; }
    public string Urgencia { get; set; } = "Não urgente";
    public DateTime? Insercao { get; set; }
    public DateTime Prazo { get; set; }
    public string Estado { get; set; }
    public bool EhAceito { get; set; } = true;

    public int IDUsuario { get; set; }
    public Usuarios Usuario { get; set; }
}
