using AutoMapper;
using BookStore.Application.DTOs.ReviewDtos;
using BookStore.Domain.Entities.Reviews;

namespace BookStore.Application.Mappings;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentRewievId));

        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
    }
}

