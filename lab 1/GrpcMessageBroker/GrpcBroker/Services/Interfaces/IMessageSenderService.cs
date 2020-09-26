using GrpcBroker.DTO;
using System.Threading.Tasks;

namespace GrpcBroker.Services.Interfaces
{
    public interface IMessageSenderService
    {
        Task SendMessageAsync(Message message);
    }
}
