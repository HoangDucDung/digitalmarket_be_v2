using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Domain.Managers
{
    public class ManagerBase
    {
        protected ILazyloadProvider _lazyloadProvider { get; }
        public ManagerBase(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }
    }
}
