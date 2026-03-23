using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Models.Business;
using Project.DigitalMarket.Domain.Repositories.Business;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Managers.Business
{
    /// <summary>
    /// Manager xử lý logic nghiệp vụ cho Seller (Domain layer)
    /// </summary>
    public class SellerManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), ISellerManager
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
                throw new BusinessException("Tài khoản không tồn tại.");
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

            // Cập nhật JSON Roles
            if (user.UserRoles.IsNullOrEmpty() || user.UserRoles == "[]")
            {
                user.UserRoles = $"[\"{RoleConstants.Seller}\"]";
            }
            else if (!user.UserRoles.Contains(RoleConstants.Seller))
            {
                user.UserRoles = user.UserRoles.Replace("]", $", \"{RoleConstants.Seller}\"]");
            }
            
            await _userManager.UpdateAsync(user);

            // 2. Tạo hoặc cập nhật hồ sơ KYC qua Repository
            var kycProfile = await _kycRepository.GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            
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
            kycProfile.CreatedAt = GenerateExtentions.Now;

            if (isNewKyc)
                await _kycRepository.AddAsync(kycProfile);
            else
                _kycRepository.Update(kycProfile);

            // 3. Tạo thông tin tài chính (Payout) qua Repository
            var financialTie = new UserFinancialTieEntity
            {
                UserId = userId,
                Type = registerDto.PayoutType,
                Provider = registerDto.PayoutProvider,
                AccountName = registerDto.PayoutAccountName,
                AccountNumber = registerDto.PayoutAccountNumber,
                IsDefault = true,
                CreatedAt = GenerateExtentions.Now
            };
            await _financialRepository.AddAsync(financialTie);

            // 4. Lưu tất cả thay đổi qua Repository
            await _kycRepository.SaveChangesAsync();
            await _financialRepository.SaveChangesAsync();
        }
    }
}
