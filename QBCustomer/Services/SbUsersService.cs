using Microsoft.EntityFrameworkCore;
using QBCustomer.Models;

namespace QBCustomer.Services
{

   
    public class SbUsersService
    {
        private SmartBooksContext _db;
        public SbUsersService(SmartBooksContext db)
        {
            _db = db;
        }
        public Task<SBUser> GetUser(string userId)
        {
            var sbUser = _db.sbUsers.FirstOrDefaultAsync(c => c.UserId == userId);
            return sbUser;
        }
    }
}
