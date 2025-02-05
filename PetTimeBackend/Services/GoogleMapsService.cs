using Google.Maps;
using Google.Maps.DistanceMatrix;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
public class GoogleMapsService
{
    private readonly string _apiKey;

    public GoogleMapsService(IConfiguration configuration)
    {
        _apiKey = configuration["GoogleMaps:ApiKey"];
    }

    public int GetDistance(double originLat, double originLng, double destLat, double destLng)
    {
        GoogleSigned.AssignAllServices(new GoogleSigned(_apiKey));

        var request = new DistanceMatrixRequest();
        request.AddOrigin(new Waypoint((decimal)originLat, (decimal)originLng));
        request.AddDestination(new Waypoint((decimal)destLat, (decimal)destLng));
        request.Mode = TravelMode.driving;

        var response = new DistanceMatrixService().GetResponse(request);
        var element = response.Rows.FirstOrDefault()?.Elements.FirstOrDefault();

        if (element != null && element.Status == ServiceResponseStatus.Ok)
        {
            if (int.TryParse(element.distance.Value, out int distance))
            {
                return distance; 
            }
            else
            {
                throw new Exception("Failed to parse distance value from Google Maps API response");
            }
        }

        throw new Exception("Failed to get distance from Google Maps API");
    }
}