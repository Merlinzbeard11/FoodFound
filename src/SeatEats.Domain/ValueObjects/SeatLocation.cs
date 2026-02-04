namespace SeatEats.Domain.ValueObjects;

public record SeatLocation
{
    public string Section { get; init; } = string.Empty;
    public string Row { get; init; } = string.Empty;
    public int SeatNumber { get; init; }

    public string DisplayText => $"Section {Section}, Row {Row}, Seat {SeatNumber}";

    public static SeatLocation Create(string section, string row, int seatNumber)
    {
        if (string.IsNullOrWhiteSpace(section))
            throw new ArgumentException("Section is required", nameof(section));
        if (string.IsNullOrWhiteSpace(row))
            throw new ArgumentException("Row is required", nameof(row));
        if (seatNumber < 1)
            throw new ArgumentException("Seat number must be positive", nameof(seatNumber));

        return new SeatLocation
        {
            Section = section.Trim().ToUpperInvariant(),
            Row = row.Trim().ToUpperInvariant(),
            SeatNumber = seatNumber
        };
    }
}
