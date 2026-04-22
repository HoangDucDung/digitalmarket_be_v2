using Microsoft.AspNetCore.Identity;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Models.Business.Seller;
using Project.DigitalMarket.Domain.Repositories.Business.Seller;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Managers.Business.Seller
{
    /// <summary>
    /// Manager xử lý logic nghiệp vụ cho Seller (Domain layer)
    /// </summary>
    internal sealed class SellerManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), ISellerManager
    {
        private UserManager<UserEntity> _userManager => _lazyloadProvider.LazyGetRequiredService<UserManager<UserEntity>>();
        private RoleManager<IdentityRole<Guid>> _roleManager => _lazyloadProvider.LazyGetRequiredService<RoleManager<IdentityRole<Guid>>>();
        private IKycRepository _kycRepository => _lazyloadProvider.LazyGetRequiredService<IKycRepository>();
        private IFinancialRepository _financialRepository => _lazyloadProvider.LazyGetRequiredService<IFinancialRepository>();

        public async Task RegisterAsSellerAsync(Guid userId, SellerRegisterReq registerDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new BusinessException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại.");
            }

            // 1. Cập nhật Bio và Role cho User
            user.Bio = registerDto.Bio;
            user.UpdatedAt = GenerateExtentions.Now;

            const string sellerRole = RoleConstants.Seller;
            if (!await _roleManager.RoleExistsAsync(sellerRole))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(sellerRole));
            }
            if (!await _userManager.IsInRoleAsync(user, sellerRole))
            {
                await _userManager.AddToRoleAsync(user, sellerRole);
            }

            await _userManager.UpdateAsync(user);

            // 2. Tạo hoặc cập nhật hồ sơ KYC qua Repository
            var kycProfile = await _kycRepository.GetByUserIdAsync(userId);
            
            bool isNewKyc = false;
            if (kycProfile == null)
            {
                isNewKyc = true;
                kycProfile = new UserKycProfileEntity { UserId = userId };
            }

            kycProfile.DocumentType = registerDto.DocumentType;
            kycProfile.DocumentNumber = registerDto.DocumentNumber;
            kycProfile.FrontImageUrl = registerDto.FrontImageUrl;
            kycProfile.BackImageUrl = registerDto.BackImageUrl;
            kycProfile.TaxId = registerDto.TaxId;
            kycProfile.VerificationStatus = KycConstants.Pending;

            if (isNewKyc)
                await _kycRepository.AddAsync(kycProfile);
            else
            {
                kycProfile.UpdatedAt = GenerateExtentions.Now;
                _kycRepository.Update(kycProfile);
            }

            // 3. Tạo hoặc cập nhật thông tin tài chính (Payout) qua Repository
            var financialTie = await _financialRepository.GetDefaultByUserIdAsync(userId);

            if (financialTie == null)
            {
                financialTie = new UserFinancialTieEntity
                {
                    UserId = userId,
                    IsDefault = true
                };
                financialTie.Type = registerDto.PayoutType;
                financialTie.Provider = registerDto.PayoutProvider;
                financialTie.AccountName = registerDto.PayoutAccountName;
                financialTie.AccountNumber = registerDto.PayoutAccountNumber;
                await _financialRepository.AddAsync(financialTie);
            }
            else
            {
                financialTie.Type = registerDto.PayoutType;
                financialTie.Provider = registerDto.PayoutProvider;
                financialTie.AccountName = registerDto.PayoutAccountName;
                financialTie.AccountNumber = registerDto.PayoutAccountNumber;
                financialTie.UpdatedAt = GenerateExtentions.Now;
                _financialRepository.Update(financialTie);
            }

            // 4. Lưu tất cả thay đổi qua Repository (Cả 2 repo dùng chung 1 DbContext)
            await _kycRepository.SaveChangesAsync();
        }
    }
}
