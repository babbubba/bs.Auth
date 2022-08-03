namespace bs.Datatable.Interfaces
{
    public interface IPageResponse<T>
    {
        T[] Data { get; set; }
        int Draw { get; set; }
        string Error { get; set; }
        int RecordsFiltered { get; set; }
        int RecordsTotal { get; set; }
    }
}