using Application.Dto;
using CsvHelper.Configuration;

namespace Application.Map
{
    public class StockMap : ClassMap<StockDto>
    {
        public StockMap()
        {
            Map(m => m.Symbol).Index(0).Name("Symbol");

            Map(m => m.Open).Index(3).Name("Open");
        }
    }
}
