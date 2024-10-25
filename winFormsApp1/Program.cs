using log4net.Config;
using log4net;
using System.Reflection;
using WinFormsApp1.Service;
using WinFormsApp1.Repository;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (!File.Exists("log4Net.config"))
            {
                throw new FileNotFoundException("log4net.config not found");
            }
            
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4Net.config"));
            //var genreService = new GenreService(new GenreRepository(new CinemaAppDbcontext()));
            var movieService = new MovieServices(new MovieRepository(new CinemaAppDbcontext()), new GenreRepository(new CinemaAppDbcontext()), new CinemaAppDbcontext());
            //genreService.ImportDataAsync().Wait();
           
            movieService.ImportDataAsync().Wait();

            
            Application.Run(new Forms.SeatSelection());
        }

        static void SeatInitialize()
        {

        }

        static void RoomInitialize()
        {
            // Create rooms
            var rooms = new List<Room>
                        {
                            new Room { Name = "Room 1", Capacity = 50 },
                            new Room { Name = "Room 2", Capacity = 75 },
                            new Room { Name = "Room 3", Capacity = 100 }
                        };

        }
    }
}