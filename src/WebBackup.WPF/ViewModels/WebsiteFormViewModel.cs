using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;

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
                websiteForm = messenger.Response;
            }
        }

        [ObservableProperty]
        private WebsiteForm? websiteForm;

        //private void Receive(WebsiteChangedMessage message)
        //{
        //    Website = message.Value;
        //}

        // TODO: disabled Save button on error
        [ICommand]
        private Task SaveAsync(WebsiteForm websiteForm)
        {
            if (websiteForm.HasErrors)
            {
                return Task.CompletedTask;
            }
            Website website = new Website { Name = websiteForm.Name, Url = websiteForm.Url };

            bool exists = _repository.ExistsAsync(websiteForm.Id).Result;
            if (exists)
            {
                website.Id = websiteForm.Id;
                string[] excludeProperties = { "FTPConnection", "SQLConnection" };
                return _repository.UpdateAsync(website, excludeProperties);
            }
            else
            {
                return _repository.InsertAsync(website);
            }
            // TODO: notify main collection
        }
    }
}
