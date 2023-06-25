using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Repository.Filter.FilterUtility;

namespace Core.Repository.Filter
{
    public class FilterState
    {
        public List<FilterModel> Filters { get; set; }  
        public List<SortParam> Sort { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public FilterState()
        {
            Filters = new List<FilterModel>();
            Sort = new List<SortParam>();
        }
    }

    public class FilterModel
    {
        public List<FilterParams> Filters { get; set; }
        public string Logic { get; set; }
        public FilterModel()
        {
            Filters = new List<FilterParams>();
        }
    }

    public class FilterParams
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public FilterOptions Operator { get; set; } = FilterOptions.contains;
    }

    public class SortParam
    {
        public string Dir { get; set; } //asc/desc
        public string Field { get; set; }
    }

    public class FilterUtility
    {
        public enum FilterOptions
        {
            startwith = 1,
            endwith,
            contains,
            doesnotcontain,
            isempty,
            isnotempty,
            gt, //is greater than
            lt, //less than
            gte, //greater than or equal
            lte, //less than or equal
            eq, //equal
            neq, //not equal
            isnull,
            isnotnull
        }
        public enum FilterLogic
        {
            And,
            Or
        }

    }

}
