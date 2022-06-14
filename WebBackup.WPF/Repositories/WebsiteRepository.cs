using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBackup.WPF.Data;
using WebBackup.WPF.Models;

namespace WebBackup.WPF.Repositories
{
    public class WebsiteRepository
    {
        public WebsiteRepository(WBContext context)
        {
            _context = context;
        }

        private readonly WBContext _context;

        public async Task<List<Website>> GetAll()
        {
            return await _context.Websites.ToListAsync();
        }
    }
}
