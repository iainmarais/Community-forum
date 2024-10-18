namespace RestApiServer.Database.Utils
{
    public class DbUtils
    {
        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        //Context-related utility methods
        //Convert enum value to string
        public static string EnumNameToString<T>(T value)
        {
            if (value == null)
            {
                throw new Exception("Value can't be null");
            }
            return Enum.GetName(typeof(T), value) ?? throw new Exception($"Conversion failed for enum value: {value}");
        }

        public static string? EnumNameToStringNullable<T>(T? value)
        {
            if (value == null)
            {
                return null;
            }
            return Enum.GetName(typeof(T), value);
        }

        //Parse enum value from string
        public static T ParseEnumFromString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value) ?? throw new Exception($"Enum value can't be null.");
        }

        public static T? ParseEnumFromStringNullable<T>(string? value)
        {
            if (value == null)
            {
                return default;
            }
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}