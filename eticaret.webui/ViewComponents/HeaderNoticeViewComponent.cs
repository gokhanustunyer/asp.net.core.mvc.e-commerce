using eticaret.business.Abstract.Service;
using Microsoft.AspNetCore.Mvc;

namespace eticaret.webui.ViewComponents
{
    public class HeaderNoticeViewComponent: ViewComponent
    {
        private readonly IPageStringsService _pageStringsService;

        public HeaderNoticeViewComponent(IPageStringsService pageStringsService)
        {
            _pageStringsService = pageStringsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _pageStringsService.GetNotices();
            return View(model);
        }
    }
}
