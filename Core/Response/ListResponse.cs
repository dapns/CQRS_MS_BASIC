﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Response
{
    /// <summary>
    /// Provides support for pagination of list objects
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ListResponse<TModel> : BaseResponse, IListResponse<TModel> where TModel : class
    {
        public ListResponse(List<TModel> model, List<KeyValuePair<string, string[]>> validationErrors = null) : base(validationErrors)
        {
            Data = model;
        }

        public int Size { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public List<TModel> Data { get; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
