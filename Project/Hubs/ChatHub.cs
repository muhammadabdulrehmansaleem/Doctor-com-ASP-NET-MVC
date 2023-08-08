using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(int recipientId, string message, int senderId)
        {
            var chatMessage = new Chat
            {
                PatientId = senderId,
                DoctorId = recipientId,
                MessageContent = message,
                MessageTimestamp = DateTime.Now
            };

            using (var dbContext = new DoctorComContext())
            {
                dbContext.Chats.Add(chatMessage);
                await dbContext.SaveChangesAsync();
            }
            await Clients.User(recipientId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }

    }
}
