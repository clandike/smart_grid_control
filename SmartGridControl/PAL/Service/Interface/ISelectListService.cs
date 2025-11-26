using Microsoft.AspNetCore.Mvc.Rendering;

namespace PAL.Service.Interface
{
    public interface ISelectListService
    {
        Task<IEnumerable<SelectListItem>> GetStatesAsync();
        Task<IEnumerable<SelectListItem>> GetTypesAsync();
        Task<IEnumerable<SelectListItem>> GetPrioritiesAsync();
    }
}
