using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.SignalR.Messaging;

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
                await Clients.All.SendAsync("ReceiveMessage", recipientId, message);

            }
            catch
            {
                Console.WriteLine($"Error sending message:");
            }
            
        }
        public async Task GetMessages(int recipientId, int senderId)
        {
            Console.WriteLine("Function caleded");
            try
            {
                using (var dbContext = new DoctorComContext())
                {
                    var patientMessages = await dbContext.Chats
                         .Where(c => (c.PatientId == senderId && c.DoctorId == recipientId) || (c.PatientId == recipientId && c.DoctorId == senderId))
                          .OrderBy(c => c.MessageTimestamp)
                          .ToListAsync();
                    foreach (var mess in patientMessages)
                        {
                            Console.WriteLine(mess.MessageContent);
                        }
                        await Clients.All.SendAsync("LoadChat", senderId, patientMessages);
                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting messages: {ex.Message}");
            }
        }


    }
}
