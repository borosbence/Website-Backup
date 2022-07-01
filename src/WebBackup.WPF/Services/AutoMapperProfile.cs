using AutoMapper;
using WebBackup.Core;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF.Services
{
	public class WBProfile : Profile
	{
		public WBProfile()
		{
			CreateMap<WebsiteVM, Website>();
			CreateMap<Website, WebsiteVM>();
			CreateMap<FTPConnection, FTPConnectionVM>();
			CreateMap<SQLConnection, SQLConnectionVM>();
		}
	}
}
