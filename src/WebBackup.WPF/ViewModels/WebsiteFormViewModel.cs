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
            var messenger = Messenger.Send<WebsiteRequestMessage>();
            if (messenger.HasReceivedResponse)
            {
                websiteForm = messenger.Response;
            }
        }

        [ObservableProperty]
        private WebsiteForm? websiteForm;

        // TODO: disabled Save button on error
        [ICommand]
        private async Task SaveAsync(WebsiteForm websiteForm)
        {
            if (websiteForm.HasErrors)
            {
                return;
            }
            Website website = new() { Name = websiteForm.Name, Url = websiteForm.Url };

            bool exists =  await _repository.ExistsAsync(websiteForm.Id);
            if (exists)
            {
                // TODO: keep tracking the entitiy
                website.Id = websiteForm.Id;
                await _repository.UpdateAsync(website);
            }
            else
            {
                await _repository.InsertAsync(website);
            }
            // notify main collection
            Messenger.Send(new WebsiteChangedMessage(website));
        }
    }
}
