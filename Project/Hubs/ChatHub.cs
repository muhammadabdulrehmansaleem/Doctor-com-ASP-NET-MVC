using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.SignalR.Messaging;

namespace Project.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(int recipientId, string message, int senderId,string user_role)
        {
            try
            {
                var chatMessage = new Chat
                {
                    PatientId = senderId,
                    DoctorId = recipientId,
                    MessageContent = message,
                    MessageTimestamp = DateTime.Now,
                    UserRole=user_role
                };
                using (var dbContext = new DoctorComContext())
                {
                    dbContext.Chats.Add(chatMessage);
                    await dbContext.SaveChangesAsync();
                }
                await Clients.All.SendAsync("ReceiveMessageDoctor", recipientId, chatMessage);
            }
            catch
            {
                Console.WriteLine($"Error sending message:");
            }
            
        }
        public async Task GetMessages(int recipientId, int senderId, string user_role)
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
                    foreach(var msg in patientMessages)
                    {
                        Console.WriteLine(msg.MessageContent);
                    }

                   if(user_role=="Patient")
                    {
                        await Clients.All.SendAsync("LoadChatPatient", senderId, patientMessages);
                        Console.WriteLine("yes here");
                    }
                   else if(user_role=="Doctor")
                    {
                        await Clients.All.SendAsync("LoadChatDoctor", senderId, patientMessages);
                    }
                    
                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting messages: {ex.Message}");
            }
        }


    }
}
