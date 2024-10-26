using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WinFormsApp1.MovieBooking;
using System.Drawing;
using WinFormsApp1.Models;

/*public class MovieDetailsForm : Form
{
    private const string API_KEY = "ecfcd92abcf0735492247a8eae3c742a"; 
    private Movie movie;

    public MovieDetailsForm(Movie movie)
    {
        this.movie = movie;

        // Initialize form properties
        this.Text = "Movie Details";
        this.Size = new Size(500, 500);
        this.AutoScroll = true;

       
    }

    /*private async Task LoadMovieDetailsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://api.themoviedb.org/3/movie/{movie.Id}?api_key={API_KEY}&language=en-US&append_to_response=credits";
            var response = await client.GetStringAsync(url);
            JObject data = JObject.Parse(response);

            movie.Runtime = data["runtime"] != null ? (int)data["runtime"] : 0;
            JArray castArray = (JArray)data["credits"]["cast"];
            movie.Cast = new List<string>();
            foreach (var castMember in castArray)
            {
                movie.Cast.Add((string)castMember["name"]);
                if (movie.Cast.Count >= 5) break;
            }

            DisplayMovieDetails();
        }
    }*/

   /* private void DisplayMovieDetails()
    {
        Label titleLabel = new Label();
        titleLabel.Text = "Title: " + movie.Title;
        titleLabel.Location = new Point(10, 10);
        titleLabel.Size = new Size(400, 30);
        this.Controls.Add(titleLabel);

        /*Label runtimeLabel = new Label();
        runtimeLabel.Text = "Runtime: " + movie.Runtime + " minutes";
        runtimeLabel.Location = new Point(10, 50);
        runtimeLabel.Size = new Size(400, 30);
        this.Controls.Add(runtimeLabel);*/

        /*Label overviewLabel = new Label();
        overviewLabel.Text = "Overview: " + movie.Overview;
        overviewLabel.Location = new Point(10, 90);
        overviewLabel.Size = new Size(400, 60);
        overviewLabel.AutoSize = true;
        this.Controls.Add(overviewLabel);

        /*Label castLabel = new Label();
        castLabel.Text = "Main Cast: " + string.Join(", ", movie.Cast);
        castLabel.Location = new Point(10, 160);
        castLabel.Size = new Size(400, 30);
        this.Controls.Add(castLabel);*/

       /* Button selectShowtimeButton = new Button();
        selectShowtimeButton.Text = "Select Showtime and Seats";
        selectShowtimeButton.Location = new Point(10, 200);
        selectShowtimeButton.Click += (s, e) =>
        {
            ShowtimeSelectionForm showtimeForm = new ShowtimeSelectionForm(movie);
            showtimeForm.ShowDialog();
        };
        this.Controls.Add(selectShowtimeButton);
    }
}
*/