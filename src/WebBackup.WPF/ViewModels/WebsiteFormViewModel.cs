using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.DataAnnotations;
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
                // websiteForm = messenger.Response;
            }
        }

        private WebsiteForm websiteForm = new();
        public WebsiteForm WebsiteForm
        {
            get { return websiteForm; }
            set { websiteForm = value; }
        }

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
            return Task.CompletedTask;

            //bool exists = _repository.ExistsAsync(website.Id).Result;
            //if (exists)
            //{
            //    return _repository.UpdateAsync(website);

            //}
            //else
            //{
            //    return _repository.InsertAsync(website);
            //}
            // TODO: notify main collection
        }
    }
}
