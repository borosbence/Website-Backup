using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.WPF.Services;

namespace WebBackup.WPF.ViewModels
{
    [ObservableRecipient]
    public partial class WebsiteFormViewModel : ObservableValidator
    {
        private readonly IGenericRepository<Website> _repository;
        private readonly IWindowService _windowService;
        public WebsiteFormViewModel(IGenericRepository<Website> repository, IWindowService windowService)
        {
            Website? selected = WeakReferenceMessenger.Default.Send<WebsiteRequestMessage>().Response;
            if (selected != null)
            {
                _website = selected;
                // TODO: localize
                Title = "Edit Website";
            }
            _repository = repository;
            _windowService = windowService;
            SaveCommand = new AsyncRelayCommand<object>(SaveAsync, CanSave);
            firstOpen = true;
        }

        /// <summary>
        /// Title of the Dialog window.
        /// </summary>
        // TODO: localize
        [ObservableProperty]
        private string title = "New Website";
        /// <summary>
        /// This is the first opening of the dialog.
        /// </summary>
        private bool firstOpen;

        private readonly Website _website = new();

        private int Id => _website.Id;

        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get => _website.Name;
            set {
                SetProperty(_website.Name, value, _website, (w, name) => w.Name = name, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        [CustomValidation(typeof(WebsiteFormViewModel), nameof(ValidateUrl))]
        public string? Url
        {
            get => _website.Url;
            set
            {
                SetProperty(_website.Name, value, _website, (w, url) => w.Url = url, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public IAsyncRelayCommand<object> SaveCommand { get; }

        private async Task SaveAsync(object window)
        {
            // TODO: create ctor?
            // var website = new Website { Id = Id, Name = Name, Url = Url};
            var website = _website;
            bool exists = await _repository.ExistsAsync(website.Id);
            if (exists)
            {
                await _repository.UpdateAsync(website);
            }
            else
            {
                await _repository.InsertAsync(website);
                website.Id = website.Id;
            }
            // Notify collection
            WeakReferenceMessenger.Default.Send(new WebsiteChangedMessage(website));
            _windowService.Close(window);
        }

        /// <summary>
        /// Validate the form and enable the Save button. On the first open, the input fields errors not shown.
        /// </summary>
        /// <returns>Form is valid.</returns>
        public bool CanSave(object window)
        {
            // if (firstOpen && id == 0)
            if (firstOpen && Id == 0)
            {
                return firstOpen = false;
            }
            ValidateAllProperties();
            return !HasErrors;
        }

        public static ValidationResult? ValidateUrl(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                bool isUri = Uri.IsWellFormedUriString(url, UriKind.Absolute);
                if (!isUri)
                {
                    // TODO: localize
                    return new("The Url field is not a valid fully-qualified http or https URL.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
