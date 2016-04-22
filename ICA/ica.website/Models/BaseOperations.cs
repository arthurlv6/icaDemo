using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace ica.website.Models
{
    public interface IBaseOperations
    {
        T AddOneEntity<T>(T input) where T : class, new();
        bool DeleteOneEntity<T>(object key) where T : class;
        void Dispose();
        T FindDetail<T>(object keyValues) where T : class;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class;
        DateTime GetNzTime();
        ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition, Expression<Func<T, TOrderBy>> orderBy, string[] includes = null, bool httpget = true) where T : class;
        void UpdateOneEntity<T>(T entity) where T : class, new();
        DateTime NewZealandTime { get; }
        void SaveChange();
        void EntityCloneNew<T>(T entity) where T : class, new();
        ApplicationDbContext GetTranscaiton ();
        string GetImage(string content, string type);

    }
    public class BaseOperations : IDisposable, IBaseOperations
    {
        protected readonly ApplicationDbContext _db;
        public BaseOperations()
        {
            _db=new ApplicationDbContext();
        }
        public ApplicationDbContext GetTranscaiton()
        {
            return _db;
        }
        public void SaveChange()
        {
            _db.SaveChanges();
        }
        public DateTime NewZealandTime
        {
            get
            {
                DateTime serverTime = DateTime.Now;
                TimeZoneInfo timeZone1 = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
                return TimeZoneInfo.ConvertTime(serverTime, timeZone1);
            }
        }
        public string GetImage(string content, string type)
        {
            var defaultImage = _db.WebDefaultImages.Find(type);
            string image= defaultImage.Image;
            //string style = defaultImage.Style;
            if (!string.IsNullOrEmpty(content))
            {
                var start = content.IndexOf("<img");
                if (start >= 0)
                {
                    var end = content.Substring(start).IndexOf(">");
                    if (end > 0)
                    {
                        image = content.Substring(start, end+1);
                        var styleStart = image.IndexOf("style");
                        if(styleStart>=0)
                            image = image.Substring(0,styleStart)+" >";
                    }
                }
            }
            return image;
        }
        public DateTime GetNzTime()
        {
            return NewZealandTime;
        }
        public virtual ConditionViewModel<T> GetSearchResult<T, TOrderBy>(ConditionViewModel<T> condition,Expression<Func<T, TOrderBy>> orderBy,  string[] includes = null, bool httpget = true) where T : class
        {
            var result = new ConditionViewModel<T>();
            SortOrder sortOrder = SortOrder.Descending;
            if (httpget)
            {
                if (condition.CurrentPage == 0 || condition.PerPageSize==0)
                {
                    condition.CurrentPage = 1;
                    condition.PerPageSize = 10;
                }
            }
            else
            {
                if (condition.CurrentPage == 0) //search
                {
                    condition.PerPageSize = 10;
                    condition.CurrentPage = 1;
                }
            }
            if (condition.ChangeOrderDirection)
            {
                if (condition.OrderDirection == "desc")
                {
                    sortOrder = SortOrder.Descending;
                    condition.OrderDirection = "asc";
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                    condition.OrderDirection = "desc";
                }
            }
            else
            {
                if (condition.OrderDirection == "desc")
                {
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                }
            }

            IQueryable<T> group;
            if (condition.Func == null)
            {
                condition.Func = d => true;
            }
            int total = _db.Set<T>().Count(condition.Func);
            var totalPages = total / condition.PerPageSize + (total % condition.PerPageSize > 0 ? 1 : 0);
            if (condition.CurrentPage > totalPages)
            {
                condition.CurrentPage = 1;
            }
            if (sortOrder == SortOrder.Ascending)
            {
                group =
                    _db.Set<T>()
                        .Where(condition.Func)
                        .OrderBy(orderBy)
                        .Skip((condition.CurrentPage - 1) * condition.PerPageSize)
                        .Take(condition.PerPageSize);
            }
            else
            {
                group =
                    _db.Set<T>()
                        .Where(condition.Func)
                        .OrderByDescending(orderBy)
                        .Skip((condition.CurrentPage - 1) * condition.PerPageSize)
                        .Take(condition.PerPageSize);
            }
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    group=group.Include(item);
                }
            }
            result.Data = group;
            result.TotalPages = totalPages == 0 ? 1 : totalPages;
            result.PerPageSize = condition.PerPageSize;
            result.CurrentPage = condition.CurrentPage;
            result.OrderDirection = condition.OrderDirection;
            result.Search = condition.Search;
            return result;
        }
        public T FindDetail<T>(object keyValues) where T : class
        {
            return _db.Set<T>().Find(keyValues);
        }
        public T AddOneEntity<T>(T input) where T : class, new()
        {
            try
            {
                _db.Set<T>().Add(input);
                _db.SaveChanges();
                return input;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                        Console.WriteLine("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                    Console.WriteLine();
                }
                throw new Exception("entity is null");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
       public void EntityCloneNew<T>(T entity) where T : class, new()
        {
            _db.Entry(entity).State = EntityState.Added;
        }
        public void UpdateOneEntity<T>(T entity) where T : class, new()
        {
            try
            {
                if (entity == null)
                    throw new Exception("entity is null");
                _db.Set<T>().Attach(entity);
                
                //DoAudit(entity, 1, "someone");
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        Console.WriteLine("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage);
                    }
                    Console.WriteLine();
                }
                throw new Exception("entity is null");
            }
            catch (Exception)
            {
                throw new Exception("entity is null");
            }
        }
        public bool DeleteOneEntity<T>(object key) where T : class
        {
            try
            {
                T existing = _db.Set<T>().Find(key);
                _db.Set<T>().Remove(existing);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> criteria) where T : class
        {
            return _db.Set<T>().Where(criteria);
        }
        protected void ThrowOperationException(Action action)
        {
            try
            {
               action.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb=new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        //Console.WriteLine();
                        sb.Append(string.Format("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                    Console.WriteLine();
                }
                throw new DbEntityValidationException(sb.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Please contact your database manager.");
            }

        }
        protected List<T> ThrowOperationException<T>(Func<List<T>> func) where T : class,new()
        {
            try
            {
               return func.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    Console.WriteLine("====================");
                    Console.WriteLine(
                        "Entity {0} in state {1} has validation errors:",
                        error.Entry.Entity.GetType().Name,
                        error.Entry.State);
                    foreach (var ve in error.ValidationErrors)
                    {
                        //Console.WriteLine();
                        sb.Append(string.Format("\tProperty: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                    Console.WriteLine();
                }
                throw new DbEntityValidationException(sb.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Please contact your database manager.");
            }

        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool dispose)
        {
            _db.Dispose();
        }
    }
}
