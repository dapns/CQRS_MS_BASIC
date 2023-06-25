﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Response
{
    /// <summary>
    /// Provides response template for single entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class SingleResponse<TModel> : BaseResponse, ISingleResponse<TModel> where TModel : class
    {
        public SingleResponse(TModel model, List<KeyValuePair<string, string[]>> validationErrors = null) : base(validationErrors)
        {
            Data = model;
        }
        public TModel Data { get; }
    }
}
