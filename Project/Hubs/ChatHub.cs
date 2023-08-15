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
            try
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
                await Clients.All.SendAsync("ReceiveMessage", senderId, message);
                Console.WriteLine($"Sent message from {senderId} to {recipientId}");
            }
            catch
            {
                Console.WriteLine($"Error sending message:");
            }
            
        }

    }
}
