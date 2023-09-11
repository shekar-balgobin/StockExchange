using AutoMapper;

namespace Tyl.StockExchange.Exchange.ViewModel;

public class MappingProfile :
	Profile {
	public MappingProfile() {
		CreateMap<Model.Trade, Trade>().ReverseMap();
		CreateMap<Model.Trade, TradeEntity>().ReverseMap();
	}
}
