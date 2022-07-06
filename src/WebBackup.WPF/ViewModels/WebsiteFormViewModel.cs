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
                id = selected.Id;
                name = selected.Name;
                url = selected.Url;
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
        [ObservableProperty]
        private string title = "New Website";
        /// <summary>
        /// This is the first opening of the dialog.
        /// </summary>
        private bool firstOpen;
        /// <summary>
        /// Website identifier.
        /// </summary>
        private readonly int id;
        /// <summary>
        /// Website name.
        /// </summary>
        private string name = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
        /// <summary>
        /// Website URL address.
        /// </summary>
        private string? url;
        [CustomValidation(typeof(WebsiteFormViewModel), nameof(ValidateUrl))]
        public string? Url
        {
            get => url;
            set
            {
                SetProperty(ref url, value, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public IAsyncRelayCommand<object> SaveCommand { get; }

        /// <summary>
        /// Validate the form and enable the Save button. On the first open, the input fields errors not shown.
        /// </summary>
        /// <returns>Form is valid.</returns>
        private bool CanSave(object window)
        {
            if (firstOpen)
            {
                return firstOpen = false;
            }
            ValidateAllProperties();
            return !HasErrors;
        }

        private async Task SaveAsync(object window)
        {
            // TODO: create ctor?
            var website = new Website { Id = id, Name = name, Url = url};
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

        public static ValidationResult? ValidateUrl(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                bool isUri = Uri.IsWellFormedUriString(url, UriKind.Absolute);
                if (!isUri)
                {
                    return new("Url is not valid.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
