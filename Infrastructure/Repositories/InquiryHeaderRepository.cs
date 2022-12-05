using Core;
using Core.Data.EntryDbModels.Inquiry;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InquiryHeaderRepository :IInquiryHeaderRepository
{
    private readonly ApplicationDbContext _db;

    public InquiryHeaderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(InquiryHeader inquiryHeader)
    {
        await _db.InquiryHeaders.AddAsync(inquiryHeader);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<InquiryHeader>> GetAll()
    {
        return await _db.InquiryHeaders.ToListAsync();
    }

    public async Task<InquiryHeader> GetById(int id)
    {
        return (await _db.InquiryHeaders.FirstOrDefaultAsync(x => x.Id == id))!;
    }
}