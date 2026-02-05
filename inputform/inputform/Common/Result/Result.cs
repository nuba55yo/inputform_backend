namespace inputform.Common.Result;

public sealed class Result<T>
{
    public bool succeeded { get; private set; }
    public string? error { get; private set; }
    public T? data { get; private set; }

    public static Result<T> Ok(T data) => new() { succeeded = true, data = data };
    public static Result<T> Fail(string error) => new() { succeeded = false, error = error };
}
