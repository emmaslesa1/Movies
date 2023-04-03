using MoviesMvc.Models.Domain;
using MoviesMvc.Repositories.Abstract;

namespace MoviesMvc.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext _context;

        public GenreService(DatabaseContext context)
        {
            _context = context;
        }
        public bool Add(Genre model)
        {
            try
            {
                _context.Genre.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                _context.Genre.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Genre GetById(int id)
        {
            return _context.Genre.Find(id);
        }

        public IQueryable<Genre> List()
        {
            var data = _context.Genre.AsQueryable();
            return data;
        }

        public bool Update(Genre model)
        {
            try
            {
                _context.Genre.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
