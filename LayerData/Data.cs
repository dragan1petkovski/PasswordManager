using DBLayer;
using DataTransferObjects.User;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace LayerData
{
    public abstract class Data
	{
		protected readonly MSSQLContext _dbContext;
		protected readonly ILogger _logger;
		protected readonly UserSession _userSession;

		public Data(MSSQLContext dbContext, ILogger logger, UserSession userSession)
		{
			this._dbContext = dbContext;
			this._logger = logger;
			this._userSession = userSession;
		}

		public int Save()
		{
			return _dbContext.SaveChanges();
		}

		public IQueryable<T> Read<T>() where T : class
		{
			try
			{
				return _dbContext.Set<T>();
				
			}
			catch (Exception ex)
			{
				_logger.Error($"Problem reading item from DB.\n{ex.Message}\n");
				return null;
			}
			
		}

		public IQueryable<T> Read<T>(Expression<Func<T,object>> includeExpression) where T : class
		{
			try
			{
				return _dbContext.Set<T>().Include(includeExpression);

			}
			catch (Exception ex)
			{
				_logger.Error($"Problem reading item from DB.\n{ex.Message}\n");
				return null;
			}

		}


		public T Read<T>(Guid id) where T: class
		{
			try
			{
				return _dbContext.Set<T>().Find(id);
			}
			catch (Exception ex)
			{
				_logger.Error($"Problem reading item from DB.\n{ex.Message}\n");
				return null;
			}

		}

        public T Read<T>(string id) where T : class
        {
            try
            {
                return _dbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Problem reading item from DB.\n{ex.Message}\n");
                return null;
            }

        }

        public bool Write<T>(List<T> itemList) where T : class
        {
            //var writeT = _dbContext.Database.BeginTransaction(); This is how to enable transactional write in database
            try
            {
                _dbContext.Set<T>().AddRange(itemList);
                _dbContext.SaveChanges();
                //This is Audit Log that need to be properly implemented
                //_logger.Warning($"{typeof(T).Name} with ID: {item.GetType().GetProperty("id").GetValue(item)?.ToString()} was successfully creted by {_userSession.email} at {_userSession.HttpRequestTime}");
                //writeT.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"DB failed to write new etnry.\n{ex.Message}\n");
                //writeT.Rollback();
                return false;
            }

        }

        public bool Write<T>(T item) where T : class
		{
			//var writeT = _dbContext.Database.BeginTransaction(); This is how to enable transactional write in database
			try
			{
				_dbContext.Set<T>().Add(item);
				_dbContext.SaveChanges();
				//This is Audit Log that need to be properly implemented
				_logger.Warning($"{typeof(T).Name} with ID: {item.GetType().GetProperty("id").GetValue(item)?.ToString()} was successfully creted by {_userSession.email} at {_userSession.HttpRequestTime}");
				//writeT.Commit();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error($"DB failed to write new etnry.\n{ex.Message}\n");
				//writeT.Rollback();
				return false;
			}
			
		}

		public bool Update<T>(T item) where T: class
		{
			try
			{
				_dbContext.Set<T>().Update(item);
				_dbContext.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error($"DB failed to update etnry.\n{ex.Message}\n");
				return false;
			}
		}
	
		public bool Delete<T>(T item) where T: class
		{
			try
			{
				_dbContext.Set<T>().Remove(item);
				_dbContext.SaveChanges();
				//This is Audit Log that need to be properly implemented
				_logger.Warning($"{typeof(T).Name} with ID: {item.GetType().GetProperty("id").GetValue(item)?.ToString()} was successfully deleted by {_userSession.email} at {_userSession.HttpRequestTime}");
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error($"DB failed to write new etnry\n{ex.Message}\n");
				return false;
			}
		}


        public bool Delete<T>(List<T> item) where T : class
        {
            try
            {
                _dbContext.Set<T>().RemoveRange(item);
                _dbContext.SaveChanges();
                //This is Audit Log that need to be properly implemented
                _logger.Warning($"{typeof(T).Name} with ID: {item.GetType().GetProperty("id").GetValue(item)?.ToString()} was successfully deleted by {_userSession.email} at {_userSession.HttpRequestTime}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"DB failed to write new etnry\n{ex.Message}\n");
                return false;
            }
        }
    }
}
