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
    public class WebsiteRepository : GenericRepository<Website, WBContext>
    {
        public WebsiteRepository(WBContext context) : base(context)
        {
        }
    }
}
