namespace TodoApp.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }

    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, Array.Empty<Error>());

    public static Result Failure(Error error) => new(false, new[] { error });

    public static Result Failure(IReadOnlyList<Error> errors) => new(false, errors);
}

// Versão genérica para quando precisamos retornar um dado
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, IReadOnlyList<Error> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Não é possível acessar o valor de um resultado de falha.");

    public static Result<TValue> Success(TValue value) => new(value, true, Array.Empty<Error>());

    public static Result<TValue> Failure(Error error) => new(default, false, new[] { error });

    public static Result<TValue> Failure(IReadOnlyList<Error> errors) => new(default, false, errors);

    // Permite implicit conversion (ex: retornar o UserDto direto e ele virar um Result<UserDto>)
    public static implicit operator Result<TValue>(TValue value) => Success(value);
}