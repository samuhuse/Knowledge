using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using static CreationalPatterns.Builders.FacadeBuilder;

namespace CreationalPatterns.Builders
{
    public class FacadeBuilder
    {
        #region Model

        public class Car
        {
            public string plate;
            public string producer;
            public double price;

            public bool hasInsurance;
            public bool hasRadio;
            public bool hasTintedWindows;
        }

        #endregion

        public class CarBuilder
        {
            protected Car car = new Car();

            public Car Build()
            {
                return car;
            }
        }

        #region Information

        public class CarInformationBuilder : CarBuilder
        {
            public CarInformationBuilder(Car car)
            {
                this.car = car;
            }
            public CarInformationBuilder Price(double price)
            {
                car.price = price;
                return this;
            }
            public CarInformationBuilder Producer(string producer)
            {
                car.producer = producer;
                return this;
            }
            public CarInformationBuilder Plate(string plate)
            {
                car.plate = plate;
                return this;
            }
        }

        #endregion

        #region Optional

        public class CarOptionalBuilder : CarBuilder
        {
            public CarOptionalBuilder(Car car)
            {
                this.car = car;
            }

            public CarOptionalBuilder HasInsurance(bool hasInsurance)
            {
                car.hasInsurance = hasInsurance;
                return this;
            }

            public CarOptionalBuilder HasRadio(bool hasRadio)
            {
                car.hasRadio = hasRadio;
                return this;
            }

            public CarOptionalBuilder HasTintedWindows(bool hasTintedWindows)
            {
                car.hasTintedWindows = hasTintedWindows;
                return this;
            }
        }

        #endregion

        [Test]
        public static void Try()
        {
            CarBuilder builder = new CarBuilder();

            Car car = builder
                        .Information()
                            .Plate("1234")
                            .Price(122)
                            .Producer("Ford")
                        .Optionals()
                            .HasTintedWindows(true)
                            .HasRadio(false)
                            .HasInsurance(true)
                        .Build();            
        }
    }

    public static class InformationExtention
    {
        public static CarInformationBuilder Information(this CarBuilder builder)
        {
            return new CarInformationBuilder(builder.Build());
        }
    }

    public static class OptionalExtention
    {
        public static CarOptionalBuilder Optionals(this CarBuilder builder)
        {
            return new CarOptionalBuilder(builder.Build());
        }
    }
}

