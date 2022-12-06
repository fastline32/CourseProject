using Core;
using Core.Data.EntryDbModels.Inquiry;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InquiryDetailsRepository : IInquiryDetailsRepository
{
    private readonly ApplicationDbContext _db;

    public InquiryDetailsRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddRangeAsync(List<InquiryDetail> items)
    {
        _db.InquiryDetails.AddRange(items);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<InquiryDetail>> GetAllAsync()
    {
        return await _db.InquiryDetails.ToListAsync();
    }
    
    public async Task<IEnumerable<InquiryDetail>> GetAllAsync(int id)
    {
        var item = await _db.InquiryDetails.Include(x => x.Product)
            .Where(x => x.InquiryHeader.Id==id)
            .ToListAsync();
        return item;
    }

    public async Task RemoveRangeAsync(IEnumerable<InquiryDetail> items)
    {
        _db.InquiryDetails.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}