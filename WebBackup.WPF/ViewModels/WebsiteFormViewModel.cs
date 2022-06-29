using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteFormViewModel : ObservableRecipient
    {
        private readonly IGenericRepository<Website> _repository;
        public WebsiteFormViewModel(IGenericRepository<Website> repository)
        {
            _repository = repository;
            // Messenger.Register<WebsiteFormViewModel, WebsiteChangedMessage>(this, (r, m) => r.Receive(m));
            var messenger = Messenger.Send<WebsiteRequestMessage>();
            if (messenger.HasReceivedResponse)
            {
                website = messenger.Response;
            }
        }

        //private void Receive(WebsiteChangedMessage message)
        //{
        //    Website = message.Value;
        //}

        [ObservableProperty]
        private Website website = new();


        // TODO: disabled Save button on error
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
