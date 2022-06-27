using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteFormViewModel : ObservableObject
    {
        private readonly IGenericRepository<Website> _repository;
        public WebsiteFormViewModel(IGenericRepository<Website> repository, Website? p_website = null)
        {
            _repository = repository;
            website = p_website ?? new Website();
        }

        [ObservableProperty]
        private Website website;

        [ICommand]
        private Task SaveAsync(Website website)
        {
            if (website.HasErrors)
            {
                return Task.CompletedTask;
            }
            bool exists = _repository.ExistsAsync(website.Id).Result;
            if (exists)
            {
                return _repository.UpdateAsync(website);

            }
            else
            {
                 return _repository.InsertAsync(website);
            }
            // TODO: notify main collection
        }
    }
}
