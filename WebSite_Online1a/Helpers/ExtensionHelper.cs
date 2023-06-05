using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Text.Json;

namespace WebSite_Online1a.Helpers
{
	public static class SessionExtensions
	{
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T? Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}
	}

	/*public static class ExtensionHelper
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
		public static void Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}
	}*/
}