using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

public interface IContactUsService
{
    Task<IEnumerable<ContactUsMessage>> GetAllMessagesAsync();
    Task<ContactUsMessage> GetMessageByIdAsync(Guid id);
    Task AddMessageAsync(ContactUsMessage message);
    Task UpdateMessageAsync(ContactUsMessage message);
    Task DeleteMessageAsync(Guid id);
}
