using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

public class ContactUsRepository : IContactUsRepository
{
    private readonly DataContext _context;

    public ContactUsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContactUsMessage>> GetAllMessagesAsync()
    {
        return await _context.ContactUsMessages.Include(message => message.User).ToListAsync();
    }

    public async Task<ContactUsMessage> GetMessageByIdAsync(Guid id)
    {
        return await _context.ContactUsMessages.FindAsync(id);
    }

    public async Task AddMessageAsync(ContactUsMessage message)
    {
        _context.ContactUsMessages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMessageAsync(ContactUsMessage message)
    {
        _context.Entry(message).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(Guid id)
    {
        var message = await _context.ContactUsMessages.FindAsync(id);
        if (message != null)
        {
            _context.ContactUsMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
