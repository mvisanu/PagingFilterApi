using PagingCustomer.Filter;
using System;

namespace PagingCustomer.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
