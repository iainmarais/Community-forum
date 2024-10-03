namespace RestApiServer.Core.Config
{
    public class EnvironmentVariable
    {
        //Class definition for an environment variable
        public string VariableName { get; private set; }
        public string? Value { get; set; }
        public string? DefaultValue { get; private set; }
        //Constructors
        private EnvironmentVariable(string varname) { VariableName = varname; }
        private EnvironmentVariable(string varname, string defaultValue) { VariableName = varname; DefaultValue = defaultValue; }
        //Some vars
        public static EnvironmentVariable JwtSharedSecret { get => new EnvironmentVariable("JWT_SHARED_SECRET"); }
        public static EnvironmentVariable JwtExpirationMins { get => new EnvironmentVariable("JWT_EXPIRATION_MINS", "750"); }
        public static EnvironmentVariable EnvironmentName { get => new EnvironmentVariable("ENVIRONMENT_NAME"); }
        public static EnvironmentVariable MySqlServer { get => new EnvironmentVariable("MYSQL_SERVER"); }
        public static EnvironmentVariable MySqlServerPort { get => new EnvironmentVariable("MYSQL_SERVER_PORT", "3306"); }
        public static EnvironmentVariable MySqlDbName { get => new EnvironmentVariable("MYSQL_DB_NAME"); }
        public static EnvironmentVariable MySqlUsername { get => new EnvironmentVariable("MYSQL_USERNAME"); }
        public static EnvironmentVariable MySqlPassword { get => new EnvironmentVariable("MYSQL_PASSWORD"); }
        public static EnvironmentVariable ShortTermCacheExpirationSecs { get => new EnvironmentVariable("SHORT_TERM_CACHE_EXPIRATION_SECS", "60"); }
        public static EnvironmentVariable AspnetCoreUrls { get => new EnvironmentVariable("ASPNETCORE_URLS"); }
    }
}