namespace VaporStore
{
	using AutoMapper;
    using System.Linq;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Export;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Username, opt => opt.MapFrom(x => x.Username))
                .ForMember(dto => dto.Purchases, opt => opt.MapFrom(x => x.Cards.SelectMany(c => c.Purchases)))
                .ForMember(dto => dto.TotalSpent, opt => opt.MapFrom(x => x.Cards.SelectMany(c => c.Purchases).Sum(v => v.Game.Price)));

            CreateMap<Purchase, PurchaseDto>().ReverseMap();

            CreateMap<Game, GameDto>().ReverseMap();

        }
	}
}