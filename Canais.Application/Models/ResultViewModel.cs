namespace Canais.Application.Models;

public class ResultViewModel
{
    public ResultViewModel(bool sucess = true, string message = "")
    {
        Sucess = sucess;
        Message = message;
    }

    public bool Sucess { get; set; }
    public string Message { get; set; }
    public static ResultViewModel Success()
    {
        return new();
    }

    public static ResultViewModel<ResultViewModel> Error(string mensagem)
    {
        return new ResultViewModel<ResultViewModel>(default, false, mensagem);
    }
}

public class ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(T? data, bool sucess = true, string message = "") : base(sucess, message)
    {
        Data = data;
    }

    public T? Data { get; set; }


    public static ResultViewModel<T> Success(T data)
    {
        return new ResultViewModel<T>(data);
    }


    public static ResultViewModel<T> Error(string mensagem)
    {
        return new ResultViewModel<T>(default, false, mensagem);
    }
}
