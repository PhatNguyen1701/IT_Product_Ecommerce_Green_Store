using AutoMapper;
using ITProductECommerce.Data;
using ITProductECommerce.ViewModels;

namespace ITProductECommerce.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<RegisterVM, Customer>();
            //    //.ForMember(c => c.CustomerName, option => option.MapFrom(RegisterVM => RegisterVM.CustomerName))
            //    //.ReverseMap();

            ////TH khác tên thuộc tính ta sẽ cấu hình chi tiết như trên

            //CreateMap<UserProfileVM, Customer>();

            CreateMap<CategoryVM, Category>().ForMember(c => c.CategoryId, opt => opt.Ignore());
        }
    }
}
