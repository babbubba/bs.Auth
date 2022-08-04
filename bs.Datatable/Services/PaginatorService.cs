using bs.Datatable.Extensions;
using bs.Datatable.Interfaces;
using bs.Datatable.ViewModels;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bs.Datatable.Services
{
    public  class PaginatorService
    {
        public IPageResponse<T> GetPage<T>(IPageRequest pageReques, IQueryable<T> source)
        {
            if(pageReques.Order != null && pageReques.Order.Count()>0)
            {
                var columnPropertyName = pageReques.Columns[pageReques.Order[0].Column].Name;
                var orderDescending = pageReques.Order[0].Dir.ToLower() == "asc" ? false : true;
                source = source.DynamicOrderBy(columnPropertyName, orderDescending);

            }

            var data = source.Skip(pageReques.Start).Take(pageReques.Length).ToArray();


            

            return new PageResponse<T>
            {
                Data = data,
                Draw = pageReques.Draw,
                RecordsFiltered = data.Count(),
                RecordsTotal = source.Count(),
            };
        }

        public IPageResponse<T> GetPage<T,T1,T2>(IPageRequest pageReques, IQueryOver<T1,T2> source)
        {
            var pagedSource = source.Skip(pageReques.Start).Take(pageReques.Length);
            var data = pagedSource.List<T>();
            return new PageResponse<T>
            {
                Data = data.ToArray(),
                Draw = pageReques.Draw,
                RecordsFiltered = data.Count(),
                RecordsTotal = source.RowCount()
            };
        }
        public IPageResponse<T> GetPage<T>(IPageRequest pageReques, ICriteria source)
        {
            var data = source.SetFirstResult(pageReques.Start).SetMaxResults(pageReques.Length).List<T>();
            return new PageResponse<T>
            {
                Data = data.ToArray(),
                Draw = pageReques.Draw,
                RecordsFiltered = data.Count(),
                RecordsTotal = source.SetProjection(Projections.RowCount()).UniqueResult<int>()
            };
        }
    }
}
