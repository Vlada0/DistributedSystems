using GrpcBroker.DTO;
using System.Threading.Tasks;

namespace GrpcBroker.Services.Interfaces
{
    public interface IMessageSenderHandlerService
    {
        Task SendMessageAsync(Message message);
    }
}
