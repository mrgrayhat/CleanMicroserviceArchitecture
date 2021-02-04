using AutoMapper;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Features.Posts.Commands.CreatePost;
using BlogModule.Application.Features.Posts.Queries.GetAllPosts;
using BlogModule.Domain.Entities;

namespace BlogModule.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Post, GetPostDto>().ReverseMap();
            CreateMap<PostLocale, PostLocaleDto>().ReverseMap(); ;

            CreateMap<GetAllPostsQuery, GetAllPostsParameter>();

            CreateMap<CreatePostCommand, Post>();

        }
    }
}
