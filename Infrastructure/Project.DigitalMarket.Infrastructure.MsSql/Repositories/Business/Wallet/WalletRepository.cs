using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths.Wallet;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Wallet
{
    internal sealed class WalletRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<WalletEntity>(lazyloadProvider), IWalletRepository
    {
    }
}
