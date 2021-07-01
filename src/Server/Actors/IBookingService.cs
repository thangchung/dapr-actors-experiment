using System.Threading.Tasks;
using Contracts;
using Microsoft.Extensions.Logging;

namespace Server.Actors
{
    public interface IBookingService
    {
        Task Book(IStorageState state, BookRequest request, bool fromActor, ILogger logger);
    }
}
