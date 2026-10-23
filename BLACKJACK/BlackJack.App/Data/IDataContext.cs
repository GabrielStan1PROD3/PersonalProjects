using BlackJack.App.Models;

namespace BlackJack.App.Data
{
    public interface IDataContext
    {
        public Task<IEnumerable<Card>> GetAllAsync();
    }
}