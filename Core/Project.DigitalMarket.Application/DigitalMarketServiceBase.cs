using AutoMapper;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application
{
    /// <summary>
    /// Lớp Base cho tất cả các Service trong Application layer
    /// </summary>
    public abstract class DigitalMarketServiceBase(ILazyloadProvider lazyloadProvider)
    {
        protected readonly ILazyloadProvider _lazyloadProvider = lazyloadProvider;

        protected IMapper _mapper => _lazyloadProvider.LazyGetRequiredService<IMapper>();
    }
}
