using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlazorApp.Model
{
    public class Room
    {
#nullable enable
        public int? Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; }
#nullable disable

        public List<RoomFeatures> Features { get; set; }
    }
}
