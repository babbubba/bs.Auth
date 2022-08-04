using bs.Datatable.Dtos;

namespace bs.Datatable.Interfaces
{
    public interface IPageRequest
    {
        Column[] Columns { get; set; }
        int Draw { get; set; }
        int Length { get; set; }
        int Start { get; set; }
        Order[] Order { get; set; }
    }
}