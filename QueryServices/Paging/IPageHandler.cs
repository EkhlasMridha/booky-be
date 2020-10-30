using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryServices.Paging
{
    public interface IPageHandler<TDocument>
    {
        public Task<IList<TDocument>> ToPagedList(IQueryable<TDocument> source, int pageSize, Expression<Func<TDocument, bool>> sortQuery);
        public Task<List<TDocument>> SearchList(IQueryable<TDocument> source, Expression<Func<TDocument, bool>> searchQuery);
    }
}
