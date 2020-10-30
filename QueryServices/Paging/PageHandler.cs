using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryServices.Paging
{
    public class PageHandler<TDocument> : IPageHandler<TDocument>
    {

		public async Task<IList<TDocument>> ToPagedList(IQueryable<TDocument> source, int pageSize,Expression<Func<TDocument,bool>> sortQuery)
        {
			var result = await source.OrderBy(sortQuery).Take(pageSize).ToListAsync();
            IList<TDocument> data = new List<TDocument>();
            data = result;

            return data;
        }

		public  Task<List<TDocument>> SearchList(IQueryable<TDocument> source, Expression<Func<TDocument,bool>> searchQuery)
        {
            var result = source.Where(searchQuery).Take(5).ToListAsync();

            return result;
        }

	}
}
