using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

public class ContactUsService : IContactUsService
{
    private readonly IContactUsRepository _repository;

    public ContactUsService(IContactUsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactUsMessage>> GetAllMessagesAsync()
    {
        return await _repository.GetAllMessagesAsync();
    }

    public async Task<ContactUsMessage> GetMessageByIdAsync(Guid id)
    {
        return await _repository.GetMessageByIdAsync(id);
    }

    public async Task AddMessageAsync(ContactUsMessage message)
    {
        await _repository.AddMessageAsync(message);
    }

    public async Task UpdateMessageAsync(ContactUsMessage message)
    {
        await _repository.UpdateMessageAsync(message);
    }

    public async Task DeleteMessageAsync(Guid id)
    {
        await _repository.DeleteMessageAsync(id);
    }
}
