namespace RestApiServer.Core.Config
{
    public static class ConfigurationLoader
    {
        private static readonly List<EnvironmentVariable> EnvVars = new()
        {

            EnvironmentVariable.EnvironmentName,
            EnvironmentVariable.MySqlServer,
            EnvironmentVariable.MySqlServerPort,
            EnvironmentVariable.MySqlDbName,
            EnvironmentVariable.MySqlUsername,
            EnvironmentVariable.MySqlPassword,
            EnvironmentVariable.JwtExpirationMins,
            EnvironmentVariable.JwtSharedSecret,
            EnvironmentVariable.ShortTermCacheExpirationSecs,
            EnvironmentVariable.AspnetCoreUrls,
            EnvironmentVariable.ServerListenPrimaryIp,
            EnvironmentVariable.ServerListenLocalhostIp,
            EnvironmentVariable.ServerHttpPort,
            EnvironmentVariable.ServerHttpsPort
        };

        private static bool IsConfigLoaded = false;

        public static void LoadConfig()
        {
            if(!IsConfigLoaded)
            {
                List<string> EncounteredErrors = new();

                if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    DotNetEnv.Env.Load("LocalEnv.cfg");
                }
                foreach (var EnvVar in EnvVars)
                {
                    var val = Environment.GetEnvironmentVariable(EnvVar.VariableName);
                    if (string.IsNullOrEmpty(val))
                    {
                        if (string.IsNullOrEmpty(EnvVar.DefaultValue))
                        {
                            EncounteredErrors.Add($"Environment variable {EnvVar.VariableName} is required.");
                        }
                    }
                    else
                    {
                        EnvVar.Value = val;
                    }
                }
                if(EncounteredErrors.Count == 0)
                {
                    IsConfigLoaded = true;
                }
                else
                {
                    string errorMsg = string.Join("\n", EncounteredErrors);
                    throw new Exception($"Errors occurred while processing configuration file: {errorMsg}");
                }
            }
        }

        public static void SetConfigValue(EnvironmentVariable envVar, string value)
        {
            if(value == null && envVar.DefaultValue == null)
            {
                throw new Exception($"Environment variable {envVar.VariableName} has no default value and no value provided.");
            }
            if(value == null && envVar.DefaultValue != null)
            {
                Console.WriteLine($"Setting default value for {envVar.VariableName} to {envVar.DefaultValue}");
                envVar.Value = envVar.DefaultValue;
            } 
            envVar.Value = value;
            Environment.SetEnvironmentVariable(envVar.VariableName, value);
        }

        public enum EnvironmentName
        {
            Local,
            Testing,
            Production
        }

        public static EnvironmentName GetEnvironmentName()
        {
            var env = GetConfigValue(EnvironmentVariable.EnvironmentName);
            switch (env)
            {
                case "Local":
                    return EnvironmentName.Local;
                case "Testing":
                    return EnvironmentName.Testing;
                case "Production":
                    return EnvironmentName.Production;
                default:
                    Console.WriteLine($"Unknown environment: {env}, defaulting to Local");
                    return EnvironmentName.Local;
            }
        }

        public static string GetConfigValue(EnvironmentVariable envVar)
        {
            var loadedValue = EnvVars.Single(ev => ev.VariableName == envVar.VariableName);
            if(string.IsNullOrEmpty(loadedValue.Value))
            {
                if(string.IsNullOrEmpty(envVar.DefaultValue))
                {
                    throw new($"Environment variable not set: {envVar.VariableName}");
                }
                else
                {
                    return envVar.DefaultValue;
                }
            }
            return loadedValue.Value;
        }

        //For handling specific types of vars other than strings:
        public static int GetConfigValueAsInt(EnvironmentVariable envVar) => int.Parse(GetConfigValue(envVar));
        public static bool GetConfigValueAsBool(EnvironmentVariable envVar) => bool.Parse(GetConfigValue(envVar));
        public static decimal GetConfigValueAsDecimal(EnvironmentVariable envVar) => decimal.Parse(GetConfigValue(envVar));

    }
}