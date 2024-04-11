using Mango.Services.Data;

namespace Mango.Services.Business.Base
{
    public class BusinessBase
    {
        protected readonly DataContext _dataContext;

        protected BusinessBase(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
