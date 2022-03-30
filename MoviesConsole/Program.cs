using Microsoft.EntityFrameworkCore;
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesConsoleApp
{
    class Program
    {
        static void Main(String[] args)
        {

            MovieContext _db = new MovieContext();

            // Mostrar o nome de todos os gêneros que um determinado ator atuou
            // e o número de filmes neste gênero
            Console.WriteLine("\nGeneros que a atriz Judi Dench atuou");
            //var query14 = from p in _db.Characters
            //                             .Include("Movie")
            //                             .Include("Actor")
            //              join genero in _db.Genres on p.Movie.GenreID equals genero.GenreId
            //              where p.Actor.Name == "Judi Dench"
            //              select new
            //              {
            //                  Genero = genero.Name,
            //                  p.Movie.Title
            //              };

            var query14 = from p in _db.Characters
                                         .Include(per => per.Movie)
                                              .ThenInclude(m => m.Genre)
                                         .Include("Actor")
                              //     join genero in _db.Genres on p.Movie.GenreID equals genero.GenreId
                          where p.Actor.Name == "Judi Dench"
                          select new
                          {
                              Genero = p.Movie.Genre.Name,
                              Personagem = p.CharacterName,
                              Diretor = p.Movie.Director,
                              p.Movie.Title
                          };


            foreach (var res in query14)
            {

                Console.WriteLine("\t {0} \t {1} \t {2} \t {3}", res.Personagem, res.Genero, res.Title, res.Diretor);
            }


            var query14b = from e in query14
                           group e by e.Genero into grp
                           select new
                           {
                               Gen = grp.Key,
                               Quant = grp.Count()
                           };


            Console.WriteLine("\nTotal por genero...");
            foreach (var res in query14b)
            {

                Console.WriteLine("\t {0} \t {1}", res.Gen, res.Quant);
            }


            Console.ReadKey();


        }

    }
}
