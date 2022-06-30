using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.WPF.Services;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteFormViewModel : ObservableRecipient
    {
        private readonly IGenericRepository<Website> _repository;
        private readonly IMapper _mapper;
        private readonly IWindowService _windowService;
        public WebsiteFormViewModel(WebsiteVM? websiteVM,
            IGenericRepository<Website> repository, IMapper mapper, IWindowService windowService)
        {
            websiteForm = websiteVM ?? new();
            _repository = repository;
            _mapper = mapper;
            _windowService = windowService;
        }

        [ObservableProperty]
        private WebsiteVM websiteForm;

        // TODO: disabled Save button on error
        [ICommand]
        private async Task SaveAsync(object window)
        {
            if (websiteForm.HasErrors)
            {
                // TODO: display error messages
                return;
            }

            var website = _mapper.Map<Website>(websiteForm);
            bool exists = await _repository.ExistsAsync(websiteForm.Id);
            if (exists)
            {
                // TODO: keep tracking the entitiy
                await _repository.UpdateAsync(website);
            }
            else
            {
                await _repository.InsertAsync(website);
            }
            _windowService.Close(window);
        }
    }
}
