using Worktime.Core.Models;

namespace Worktime.Core.CRUD
{
    internal static class ResultGeneric
    {
        internal static Result Execute<T>(IEnumerable<T> items, Func<T, Result> func)
        {
            int index = 0;
            foreach (var item in items)
            {
                var result = func(item);
                if (!result.Success)
                {
                    result.Message += $" Index: {index}";
                    return result;
                }
                index++;
            }
            return new Result() { Success = true };
        }
    }
}
