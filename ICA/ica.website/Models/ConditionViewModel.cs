using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ica.website.Models
{
    public class ConditionViewModel<T> where T : class
    {
        //public ConditionViewModel()
        //{
        //    Data = new List<T>();
        //}
        public int CurrentPage { get; set; }
        public int PerPageSize { get; set; }
        public int TotalPages { get; set; }
        public int Status { get; set; }
        public string Search { get; set; }
        public string SearchTwo { get; set; }
        public string Order { get; set; }
        public bool ChangeOrderDirection { get; set; }
        public string OrderDirection { get; set; }
        public IQueryable<T> Data { get; set; }
        public Expression<Func<T, bool>> Func { get; set; }
    }
}