using Worktime.Core.Models;

namespace Worktime.Core.CRUD
{
    internal static class ResultGeneric
    {
        internal static Result<T> Execute<T>(IEnumerable<T> items, Result<T> result, Func<T, Result<T>, Result<T>> func)
        {
            int index = 0;
            foreach (var item in items)
            {
                result = func(item, result);
                if (!result.Success)
                {
                    result.Message += $" Index: {index}";
                    return result;
                }
                index++;
            }
            return result;
        }
    }
}
