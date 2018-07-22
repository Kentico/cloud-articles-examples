using System;
using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public class CustomTypeProvider : ICodeFirstTypeProvider
    {
        public Type GetType(string contentType)
        {
            switch (contentType)
            {
                case "bike_fork":
                    return typeof(BikeFork);
                case "bike_frame":
                    return typeof(BikeFrame);
                case "bottom_bracket":
                    return typeof(BottomBracket);
                case "brake_rotor":
                    return typeof(BrakeRotor);
                case "brakes":
                    return typeof(Brakes);
                case "conventional_bike":
                    return typeof(ConventionalBike);
                case "crankset":
                    return typeof(Crankset);
                case "electric_bike":
                    return typeof(ElectricBike);
                case "financial_service":
                    return typeof(FrontDerailleur);
                case "handlebar":
                    return typeof(Handlebar);
                case "rear_derailleur":
                    return typeof(RearDerailleur);
                case "saddle":
                    return typeof(Saddle);
                case "seatpost":
                    return typeof(Seatpost);
                case "shock":
                    return typeof(Shock);
                case "stem":
                    return typeof(Stem);
                case "tire":
                    return typeof(Tire);
                case "wheelset":
                    return typeof(Wheelset);
                case "office":
                    return typeof(Office);
                default:
                    return null;
            }
        }
    }
}