namespace TechTask.Request;

public record TarefaRequest (string tarefa, string descricao, string urgencia, DateTime insercao, DateTime prazo, string estado, int idUsuario);
