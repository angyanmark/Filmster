using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Filmster.Common.Helpers
{
    public static class Json
    {
        public static async Task<T> ToObjectAsync<T>(string value) =>
            await Task.FromResult(JsonConvert.DeserializeObject<T>(value));

        public static async Task<string> StringifyAsync(object value) =>
            await Task.FromResult(JsonConvert.SerializeObject(value));
    }
}
