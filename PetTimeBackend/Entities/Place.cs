﻿using Microsoft.AspNetCore.Http.Connections;

namespace PetTimeBackend.Entities
{
    public class Place
    {
        public long Id { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string GoogleMapsId { get; set; }
        public bool IsPetFriendly { get; set; }

    }
}
