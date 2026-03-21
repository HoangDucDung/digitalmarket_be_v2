using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application
{
    public abstract class DigitalMarketServiceBase
    {
        protected readonly ILazyloadProvider _lazyloadProvider;

        protected DigitalMarketServiceBase(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }
    }
}
