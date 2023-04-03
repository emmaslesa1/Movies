using MoviesMvc.Models.Domain;
using MoviesMvc.Models.DTO;
using MoviesMvc.Repositories.Abstract;

namespace MoviesMvc.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext _context;
        public MovieService(DatabaseContext context)
        {
            _context = context;
        }
        public bool Add(Movie model)
        {
            try
            {

                _context.Movie.Add(model);
                _context.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId
                    };
                    _context.MovieGenre.Add(movieGenre);
                }
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
                var movieGenres = _context.MovieGenre.Where(a => a.MovieId == data.Id);
                foreach (var movieGenre in movieGenres)
                {
                    _context.MovieGenre.Remove(movieGenre);
                }
                _context.Movie.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return _context.Movie.Find(id);
        }

        public List<int> GetGenreByMovieId(int movieId)
        {
            var genreIds = _context.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreIds;
        }

        public MovieListViewModel List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new MovieListViewModel();

            var list = _context.Movie.ToList();


            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }

            if (paging)
            {
                // here we will apply paging
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }

            foreach (var movie in list)
            {
                var genres = (from genre in _context.Genre
                              join mg in _context.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                var genresToDeleted = _context.MovieGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var mGenre in genresToDeleted)
                {
                    _context.MovieGenre.Remove(mGenre);
                }
                foreach (int genId in model.Genres)
                {
                    var movieGenre = _context.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    if (movieGenre == null)
                    {
                        movieGenre = new MovieGenre { GenreId = genId, MovieId = model.Id };
                        _context.MovieGenre.Add(movieGenre);
                    }
                }

                _context.Movie.Update(model);
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
