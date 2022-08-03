using bs.Datatable.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace bs.Datatable.ViewModels
{
    public class PageResponse<T> : IPageResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public T[] Data { get; set; }
        public string Error { get; set; }
    }
}
