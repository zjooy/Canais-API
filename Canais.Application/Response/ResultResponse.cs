namespace Canais.Application.Response;

public class ResultResponse<T>
{
    public T? Dados { get; set; }
    public string Mensagem { get; set; }
    public bool Sucesso { get; set; } = true;
}
