namespace RestApiServer.Common.Config
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
        public static EnvironmentVariable ServerListenLocalhostIp = new EnvironmentVariable("SERVER_LISTEN_LOCALHOST_IP", "127.0.0.1");
        public static EnvironmentVariable ServerListenPrimaryIp = new EnvironmentVariable("SERVER_LISTEN_PRIMARY_IP", "0.0.0.0");
        public static EnvironmentVariable ServerHttpsPort = new EnvironmentVariable("SERVER_HTTPS_PORT", "443");
        public static EnvironmentVariable ServerHttpPort = new EnvironmentVariable("SERVER_HTTP_PORT", "80");
        public static EnvironmentVariable JwtIssuer = new EnvironmentVariable("JWT_ISSUER", "ICCS");
        public static EnvironmentVariable AppName = new EnvironmentVariable("APP_NAME", "Community Forum");
    }
}