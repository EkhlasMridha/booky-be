using System;
using System.Collections.Generic;
using System.Text;

namespace QueryServices
{
    public class QueryParameters
    {
        const string defaultValue = "";
        private string _deafaultQuery = "";
		public string SearchParam {
            get
            {
                return _deafaultQuery;
            }
            set
            {
                _deafaultQuery = value!=null?value:defaultValue;
            }
        }
	}
}
