    public class OperationResult
    {
        public bool IsSuccess => Errors.Count == 0;
        public IReadOnlyList<string> Errors { get; private set; } = [];
        public static OperationResult Success() => new();
        public static OperationResult Failure(IEnumerable<string> errors) => new() { Errors = [.. errors] };
    }