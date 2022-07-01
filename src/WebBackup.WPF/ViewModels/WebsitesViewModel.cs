using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.WPF.Services;
using WebBackup.WPF.Views;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsitesViewModel : ObservableRecipient, IRecipient<WebsiteRequestMessage>
    {
        private readonly IGenericRepository<Website> _repository;
        private readonly IMapper _mapper;
        private readonly IWindowService _windowService;

        public WebsitesViewModel(IGenericRepository<Website> repository, IMapper mapper, IWindowService windowService)
        {
            _repository = repository;
            _mapper = mapper;
            _windowService = windowService;
            Task.Run(async () => await LoadData()).Wait();
            OnActivated();
        }

        public ObservableCollection<WebsiteVM> Websites { get; set; } = new ObservableCollection<WebsiteVM>();

        [ObservableProperty]
        private WebsiteVM? selectedWebsite;

        [ICommand]
        private void ShowWebsite(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                selectedWebsite = new();
            }
            //var wfVM = new WebsiteFormViewModel(selectedWebsite, _repository, _mapper, _windowService);
            //_windowService.ShowDialog<WebsiteFormWindow>(wfVM);
            _windowService.ShowDialog<WebsiteFormWindow>();
        }

        [ICommand]
        private async Task DeleteAsync(WebsiteVM selectedWebsite)
        {
            // TODO: localize
            bool confirmed = _windowService.ConfirmDelete("Delete Website", "Confirm Delete?");
            if (confirmed)
            {
                var website = _mapper.Map<Website>(selectedWebsite);
                await _repository.DeleteAsync(website);
                Websites.Remove(selectedWebsite);
            }
        }

        /// <summary>
        /// Load all websites from the repository.
        /// </summary>
        private async Task LoadData()
        {
            Websites.Clear();
            var dbList = await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection);
            foreach (var dbRecord in dbList)
            {
                var website = _mapper.Map<WebsiteVM>(dbRecord);
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
                Websites.Add(website);
            }
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebsiteChangedMessage>(this, (r, m) => Receive(m));
            Messenger.Register<WebsiteRequestMessage>(this, (r, m) => Receive(m));
        }

        private void Receive(WebsiteChangedMessage message)
        {
            var websiteVM = message.Value;
            var existing = Websites.FirstOrDefault(x => x.Id == websiteVM.Id);
            // Replace, update existing element
            if (existing != null)
            {
                var index = Websites.IndexOf(existing);
                Websites[index] = websiteVM;
            }
            // Add new element
            else
            {
                Websites.Add(websiteVM);
            }
        }

        public void Receive(WebsiteRequestMessage message)
        {
            message.Reply(selectedWebsite);
        }

    }

    public class WebsiteChangedMessage : ValueChangedMessage<WebsiteVM>
    {
        public WebsiteChangedMessage(WebsiteVM value) : base(value)
        {
        }
    }
    public class WebsiteRequestMessage : RequestMessage<WebsiteVM?>
    {

    }
}
