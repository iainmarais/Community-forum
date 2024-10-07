namespace RestApiServer.Core.Errorhandler
{
    public class ClientInducedException : Exception
    {
        public List<InputValidationError>? ValidationErrors { get; set; }
        protected ClientInducedException(string message) : base(message)
        {
            ValidationErrors = new();
        }

        protected ClientInducedException(string message, List<InputValidationError> inputValidationErrors) : base(message)
        {
            ValidationErrors = inputValidationErrors;
        }

        public static ClientInducedException MessageOnly(string message)
        {
            return new ClientInducedException(message);
        }

        public static ClientInducedException WithInputValidationError(InputValidationTarget target, string reason = "is required")
        {
            var combinedMsg = $"{target.Name} {reason}";
            var validationError = InputValidationError.CustomMessage(combinedMsg, target);
            return new ClientInducedException(combinedMsg, new List<InputValidationError> { validationError });
        }
    }

    public class InputValidationError
    {
        public string? Message { get; set; }
        public required InputValidationTarget Target { get; set; }

        public static InputValidationError RequiredDataMissing(InputValidationTarget target)
        {
            return new InputValidationError
            {
                Message = $"{target.Name} is required",
                Target = target
            };
        }
        
        public static InputValidationError CustomMessage(string message, InputValidationTarget target)
        {
            return new InputValidationError
            {
                Message = message,
                Target = target
            };
        }
    }

    public class InputValidationTarget
    {
        public enum InputValidationTargetId
        {
            Thread_Name_Input,
        }
        public InputValidationTargetId Id { get; }
        public string Name { get; }
        private InputValidationTarget(InputValidationTargetId id, string name)
        {
            Id = id;
            Name = name;
        }
        public static InputValidationTarget ThreadNameEmpty => new InputValidationTarget(InputValidationTargetId.Thread_Name_Input, "Thread name");
    }
}