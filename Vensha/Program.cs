using Vensha.Services;
using System.Threading.Tasks;

namespace Vensha
{
    internal sealed class Program
    {
        static Task Main(string[] args) => new DiscordService().InitializeAsync();
    }
}
