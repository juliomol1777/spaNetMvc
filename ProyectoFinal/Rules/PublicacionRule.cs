using Dapper;
using ProyectoFinal.Models;
using System.Data.SqlClient;

namespace ProyectoFinal.Rules
{
    public class PublicacionRule
    {
        private readonly IConfiguration _configuration;
        public PublicacionRule(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public Publicacion GetOnePostRandom()
        {
            var connectionString = _configuration.GetConnectionString("BlogDatabase");
            //var connectionString = @"Server= .\SQLEXPRESS; Database = BlogDatabase; Trusted_Connection=true ";
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var post = connection.Query<Publicacion>("SELECT TOP 1 * FROM Publicacion ORDER BY NEWID()");
                return post.First();
            }
            /*var post = new Publicacion
            {
                Titulo = "Man must explore, and this is exploration at its greatest",
                SubTitulo = "Problems look mighty small from 150 miles up",
                Cuerpo = "mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
                Creacion = new DateTime(2022, 11, 01),
                Creador = "Jul Mol",
                Imagen = "/assets/img/post-bg.jpg"
            };
            return post;*/
        }

        public List<Publicacion> GetPostToHome()
        {
            var connectionString = _configuration.GetConnectionString("BlogDatabase");
            //var connectionString = @"Server= .\SQLEXPRESS; Database = BlogDatabase; Trusted_Connection=true ";
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var post = connection.Query<Publicacion>("SELECT TOP 4 * FROM Publicacion ORDER BY creacion DESC");
                return post.ToList();
            }
        }

        public Publicacion GetPostById(int id)
        {
            var connectionString = _configuration.GetConnectionString("BlogDatabase");
            //var connectionString = @"Server= .\SQLEXPRESS; Database = BlogDatabase; Trusted_Connection=true ";
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var query = "SELECT * FROM Publicacion WHERE Id=@id";
                var post = connection.QueryFirstOrDefault<Publicacion>(query, new {id=id});
                return post;
            }
        }
    }
}