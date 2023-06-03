using Machete.Domain;

namespace Machete.Api.Helpers
{
    public static class Extensions
    {
        public static bool IsUserDefined(this Config config) =>
            UserDefinedConfigs.Lower.Contains(config.key.ToLower());
    }
}