using Microsoft.EntityFrameworkCore;
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieContext _db = new MovieContext();

            Console.WriteLine();
            Console.WriteLine("1. Todos os filmes de acao");
            Console.WriteLine("1a. Modelo tradicional");
            List<Movie> filmes1a = new List<Movie>();
            foreach (Movie f in _db.Movies.Include("Genre"))
            {
                if (f.Genre.Name == "Action")
                    filmes1a.Add(f);
            }
            foreach (Movie filme in filmes1a)
            {
                Console.WriteLine("\t{0}", filme.Title);
            }


            Console.WriteLine("\n1b. Usando linq - query syntax");
            var filmes1b = from f in _db.Movies
                           where f.Genre.Name == "Action"
                           select f;

            foreach (Movie filme in filmes1b)
            {
                Console.WriteLine("\t{0}", filme.Title);
            }


            Console.WriteLine("\n1c. Usando linq - method syntax");
            var filmes1c = _db.Movies.Where(m => m.Genre.Name == "Action");
            foreach (Movie filme in filmes1c)
            {
                Console.WriteLine("\t{0}", filme.Title);
            }
            Console.WriteLine();
            Console.WriteLine("2. Todos os diretores de filmes do genero 'Action', com projecao");

            var filmes2 = from m in _db.Movies
                          where m.Genre.Name == "Action"
                          select m.Director;
            foreach (String nome in filmes2)
            {
                Console.WriteLine("\t{0}", nome);
            }


            Console.WriteLine();
            Console.WriteLine("3a. Todos os filmes de cada genero (query syntax):");

            var generosFilmes3a = from g in _db.Genres.Include(gen => gen.Movies)
                                  select g;
            foreach (var gf in generosFilmes3a)
            {
                if (gf.Movies.Count > 0)
                {
                    Console.WriteLine("Filmes do genero: " + gf.Name);
                    foreach (var f in gf.Movies)
                    {
                        Console.WriteLine("\t{0}", f.Title);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("3b. Todos os filmes de cada genero (method syntax):");
            var generosFilmes3b = _db.Genres.Include(gen => gen.Movies).ToList();
            foreach (var gf in generosFilmes3a)
            {
                if (gf.Movies.Count > 0)
                {
                    Console.WriteLine("Filmes do genero: " + gf.Name);
                    foreach (var f in gf.Movies)
                    {
                        Console.WriteLine("\t{0}", f.Title);
                    }


                    var proj = from f in _db.Movies
                               where f.Director == "Quentin Tarantino"
                               select
                               new
                               {
                                   Titulo = f.Title,
                                   Lancamento = f.ReleaseDate
                               };
                    foreach (var elem in proj)
                        Console.WriteLine(elem.Titulo);
                }
            }

            static void Main0(string[] args)
            {
                MovieContext _context = new MovieContext();

                Genre g1 = new Genre()
                {
                    Name = "Comedia",
                    Description = "Filmes de comedia"
                };
                Genre g2 = new Genre()
                {
                    Name = "Ficcao",
                    Description = "Filmes de ficcao"
                };
                _context.Genres.Add(g1);
                _context.Genres.Add(g2);
                _context.SaveChanges();

                List<Genre> genres = _context.Genres.ToList();
                foreach (Genre g in genres)
                {
                    Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                    g.GenreId, g.Name, g.Description));
                }

                // ???
                genres[0].Description = "modificado";
                _context.SaveChanges();


            }
        }
    }
}
