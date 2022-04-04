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
            Console.WriteLine("\n1 - Listar o nome de todos personagens desempenhados por Judi Dench, incluindo título do filme e o diretor");

            var query1 = from p in _db.Characters
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


            foreach (var res in query1)
            {

                Console.WriteLine("\t {0} \t {1} \t {2} \t {3}", res.Personagem, res.Genero, res.Title, res.Diretor);
            }


            Console.WriteLine("\n2- Mostrar o nome e idade de todos atores que já atuaram como James Bond");

            var query2 = from p in _db.Characters
                                    .Include(per => per.Actor)
                         where p.CharacterName == "James Bond"
                         select new 
                         {
                             Nome = p.Actor.Name,
                             IdadeAtores = DateTime.Now.Year - p.Actor.DateBirth.Year
                         };

            foreach (var res in query2.Distinct())
            {
              
                Console.WriteLine("\t {0} \t {1}", res.Nome, res.IdadeAtores);
            }

            Console.WriteLine("\n3- Informar qual o ator desempenhou mais vezes o papel de James Bond");

            var query3 = from p in _db.Characters
                                    .Include(per => per.Actor)
                         where p.CharacterName == "James Bond"  
                         group p by p.Actor.Name into grupo
                         select new
                         {
                             NomeAtor = grupo.Key,
                             NomePersonagem = grupo.Count(),                                                         
                         };
            

            foreach (var res in query3.OrderByDescending(n => n.NomePersonagem).Take(1))
            {

                Console.WriteLine("\t {0}", res.NomeAtor);
            }


            Console.WriteLine("\n4-Mostrar o nome e a data de nascimento de todos atores que já atuaram como Darth Vader");

            var query4 = from p in _db.Characters
                                    .Include(per => per.Actor)
                         where p.CharacterName == "Darth Vader"
                         select new
                         {
                             Nome = p.Actor.Name,
                             DatadeNascimento = p.Actor.DateBirth
                         };

            foreach (var res in query4.Distinct())
            {

                Console.WriteLine("\t {0} \t {1}", res.Nome, res.DatadeNascimento);
            }

            Console.WriteLine("\n5-Mostrar o nome e a data de nascimento do ator mais novo a atuar do Gênero Action");

            var query5 = from p in _db.Characters
                                    .Include(per => per.Movie)
                                              .ThenInclude(m => m.Genre)
                                         .Include("Actor")
                         where p.Movie.Genre.Name == "Action"
                         select new
                         {
                             Nome = p.Actor.Name,
                             IdadeAtores = DateTime.Now.Year - p.Actor.DateBirth.Year,
                             DataNascimento = p.Actor.DateBirth
                         };

            foreach (var res in query5.OrderBy(i => i.IdadeAtores).Take(1))
            {

                Console.WriteLine("\t {0} \t {1}", res.Nome, res.DataNascimento);
            }

            Console.WriteLine("\n6-Mostrar o valor médio das avaliações dos filmes do ator Judi Dench");

            var query6 = from p in _db.Characters
                                         .Include(per => per.Movie)
                                           .Include(m => m.Actor).ToList()
                         where p.Actor.Name == "Judi Dench"
                         group p by p.Actor.Name into grupo
                         select new
                         {
                             Media = grupo.Average(per => per.Movie.Rating)                             
                         };


            foreach (var res in query6)
            {

                Console.WriteLine("\t Avaliação média: {0}", res.Media);
            }

            Console.WriteLine("\n7-Qual o elenco do filme melhor avaliado? ");

            var query7 = from p in _db.Characters
                                         .Include(per => per.Movie)
                                           .Include(m => m.Actor).ToList()
                         //where p.Actor.Name == "Judi Dench"
                         //group p by p.Movie.GenreID into grupo
                         select new
                         {
                             Elenco = p.Actor.Name,
                             Avaliacoes = p.Movie.Rating
                             
                             
                         };


            foreach (var res in query7.OrderByDescending(a => a.Avaliacoes).Take(7))
            {

                Console.WriteLine("\t Atores: {0}, \t Avaliação: {1} ", res.Elenco, res.Avaliacoes);
            }


            Console.WriteLine("\n8-Qual o elenco do filme com o menor faturamento? ");

            var query8 = from p in _db.Characters
                                         .Include(per => per.Movie)
                                           .Include(m => m.Actor).ToList()
                             //where p.Actor.Name == "Judi Dench"
                             //group p by p.Movie.GenreID into grupo
                         select new
                         {
                             Elenco = p.Actor.Name,
                             Faturamento = p.Movie.Gross

                         };


            foreach (var res in query8.OrderBy(a => a.Faturamento).Take(3))
            {

                Console.WriteLine("\t Atores: {0}, \t Faturamento: {1} ", res.Elenco, res.Faturamento);
            }




            //var query14b = from e in query14
            //               group e by e.Genero into grp
            //               select new
            //               {
            //                   Gen = grp.Key,
            //                   Quant = grp.Count()
            //               };


            //Console.WriteLine("\nTotal por genero...");
            //foreach (var res in query14b)
            //{

            //    Console.WriteLine("\t {0} \t {1}", res.Gen, res.Quant);
            //}


            Console.ReadKey();


        }

    }
}
