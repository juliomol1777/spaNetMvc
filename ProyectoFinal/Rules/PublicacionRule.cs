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

        public void InserPost(Publicacion data)
        {
            var connectionString = _configuration.GetConnectionString("BlogDatabase");
            //var connectionString = @"Server= .\SQLEXPRESS; Database = BlogDatabase; Trusted_Connection=true ";
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var queryInsert = "INSERT INTO Publicacion(Titulo, Subtitulo, Creador, Cuerpo, Creacion, Imagen) Values(@titulo, @subtitulo, @creador, @cuerpo, @creacion, @imagen)";
                var result = connection.Execute(queryInsert, new
                {
                    titulo = data.Titulo,
                    subtitulo = data.SubTitulo,
                    creador = data.Creador,
                    cuerpo = data.Cuerpo,
                    creacion = data.Creacion,
                    imagen = data.Imagen
                });

            }
               
        }

        public List<Publicacion> GetPublicaciones(int cant, int pagina)
        {
            var connectionString = _configuration.GetConnectionString("BlogDatabase");
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();
                var query = @$"SELECT * FROM Publicacion 
                                ORDER BY Creacion DESC 
                                OFFSET {cant*pagina} ROWS 
                                FETCH NEXT {cant} ROWS ONLY ";
                var posts = connection.Query<Publicacion>(query);

                return posts.ToList();
            }
        }
    }
}