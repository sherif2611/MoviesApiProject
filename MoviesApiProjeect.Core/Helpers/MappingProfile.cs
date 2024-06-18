using AutoMapper;
using MoviesApiProjeect.Core.Models;

namespace MoviesApiProjeect.Core.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>()
                .ForMember(dst => dst.PhoneNumber, opt => opt.Ignore())
                .ForMember(dst => dst.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dst => dst.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dst => dst.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dst => dst.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dst => dst.LockoutEnd, opt => opt.Ignore())
                .ForMember(dst => dst.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dst => dst.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dst => dst.SecurityStamp, opt => opt.Ignore())
                .ForMember(dst => dst.TwoFactorEnabled, opt => opt.Ignore())
                /*.ForMember(dst=>dst.PasswordHash,src=>src.MapFrom(src=>src.Password))*/;
        }
    }
}