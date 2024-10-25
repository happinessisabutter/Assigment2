using System;
using System.Windows.Forms;
using System.Drawing;
using MovieBooking;

public class TicketInfoForm : Form
{
    private Ticket ticket;

    public TicketInfoForm(Ticket ticket)
    {
        this.ticket = ticket;

        // Initialize form properties
        this.Text = "Ticket Information";
        this.Size = new Size(300, 350);

        DisplayTicketInfo();
    }

    private void DisplayTicketInfo()
    {
        int y = 10;

        Label ticketIdLabel = new Label();
        ticketIdLabel.Text = "Ticket ID: " + ticket.TicketId;
        ticketIdLabel.Location = new Point(10, y);
        ticketIdLabel.Size = new Size(260, 30);
        this.Controls.Add(ticketIdLabel);
        y += 40;

        Label movieLabel = new Label();
        movieLabel.Text = "Movie: " + ticket.Movie.Title;
        movieLabel.Location = new Point(10, y);
        movieLabel.Size = new Size(260, 30);
        this.Controls.Add(movieLabel);
        y += 40;

        Label dateLabel = new Label();
        dateLabel.Text = "Date: " + ticket.Date.ToString("MM/dd/yyyy");
        dateLabel.Location = new Point(10, y);
        dateLabel.Size = new Size(260, 30);
        this.Controls.Add(dateLabel);
        y += 40;

        Label showtimeLabel = new Label();
        showtimeLabel.Text = "Showtime: " + ticket.Showtime.ToString("HH:mm");
        showtimeLabel.Location = new Point(10, y);
        showtimeLabel.Size = new Size(260, 30);
        this.Controls.Add(showtimeLabel);
        y += 40;

        Label seatLabel = new Label();
        seatLabel.Text = "Seat: " + ticket.Seat;
        seatLabel.Location = new Point(10, y);
        seatLabel.Size = new Size(260, 30);
        this.Controls.Add(seatLabel);
        y += 40;

        Button closeButton = new Button();
        closeButton.Text = "Close";
        closeButton.Location = new Point(10, y);
        closeButton.Size = new Size(260, 30);
        closeButton.Click += (s, e) =>
        {
            this.Close();
        };
        this.Controls.Add(closeButton);
    }
}
