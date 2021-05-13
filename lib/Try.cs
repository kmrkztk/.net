using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Try 
    {
        Action _action;
        Action<Exception> _catch;
        Action _finally;
        public Try Catch(Action<Exception> @catch) => Of(_action, @catch, _finally);
        public Try Finally(Action @finally) => Of(_action, _catch, @finally);
        public void Invoke()
        {
            try
            {
                _action?.Invoke();
            }
            catch (Exception ex)
            {
                _catch?.Invoke(ex);
            }
            finally
            {
                _finally?.Invoke();
            }
        }
        public static Try Of(Action action) => Of(action, null, null);
        public static Try Of(Action action, Action<Exception> @catch) => Of(action, @catch, null);
        public static Try Of(Action action, Action<Exception> @catch, Action @finally) => new()
        {
            _action = action,
            _catch = @catch,
            _finally = @finally,
        };
        public static Try<T> Of<T>(Func<T> action) => Of(action, null, null);
        public static Try<T> Of<T>(Func<T> action, Func<Exception, T> @catch) => Of(action, @catch, null);
        public static Try<T> Of<T>(Func<T> action, Func<Exception, T> @catch, Action @finally) => Try<T>.Of(action, @catch, @finally);
    }
    public class Try<T>
    {
        Func<T> _action;
        Func<Exception, T> _catch;
        Action _finally;
        public Try<T> Catch(Func<Exception, T> @catch) => Of(_action, @catch, _finally);
        public Try<T> Finally(Action @finally) => Of(_action, _catch, @finally);
        public T Invoke()
        {
            try
            {
                return _action == null ? default : _action.Invoke();
            }
            catch (Exception ex)
            {
                return _catch == null ? default : _catch.Invoke(ex);
            }
            finally
            {
                _finally?.Invoke();
            }
        }
        public static Try<T> Of(Func<T> action) => Of(action, null, null);
        public static Try<T> Of(Func<T> action, Func<Exception, T> @catch) => Of(action, @catch, null);
        public static Try<T> Of(Func<T> action, Func<Exception, T> @catch, Action @finally) => new()
        {
            _action = action,
            _catch = @catch,
            _finally = @finally,
        };
    }
}
