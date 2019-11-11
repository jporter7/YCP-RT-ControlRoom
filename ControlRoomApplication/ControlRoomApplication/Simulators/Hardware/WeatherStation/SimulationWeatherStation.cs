using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Simulators.Hardware.WeatherStation
{
    public class SimulationWeatherStation : AbstractWeatherStation
    {
        private Random Rand;
        private DateTime lastRefreshTime;

        private int windDirectionCounter;
        private String[] windDirections = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S",
                                                "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"};

        private struct Weather_Data
        {
            public float windSpeed;
            public String windDirection;
            public float dailyRain;
            public float rainRate;
            public float outsideTemp;
            public float insideTemp;
            public float baromPressure;
            public float dewPoint;
            public float windChill;
            public float outsideHumidity;
            public float totalRain;
            public float monthlyRain;
            public float heatIndex;

            public Weather_Data(float windSpeedIN, String windDirectionIN, float dailyRainIN, float rainRateIN,
                                    float outsideTempIN, float insideTempIN, float baromPressureIN, float dewPointIN, float windChillIN,
                                    float outsideHumidityIN, float totalRainIN, float monthlyRainIN, float heatIndexIN)
            {
                windSpeed = windSpeedIN;
                windDirection = windDirectionIN;
                dailyRain = dailyRainIN;
                rainRate = rainRateIN;
                outsideTemp = outsideTempIN;
                insideTemp = insideTempIN;
                baromPressure = baromPressureIN;
                dewPoint = dewPointIN;
                windChill = windChillIN;
                outsideHumidity = outsideHumidityIN;
                totalRain = totalRainIN;
                monthlyRain = monthlyRainIN;
                heatIndex = heatIndexIN;
            }
        };

        Weather_Data data;

        public SimulationWeatherStation(int currentWindSpeedScanDelayMS)
            : base(currentWindSpeedScanDelayMS)
        {
            Rand = new Random();
            lastRefreshTime = (DateTime.Now).AddSeconds(-70);
            windDirectionCounter = 0;
            data = new Weather_Data(0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);


            ReloadWeatherDataThread = null;
            KeepReloadWeatherDataThreadAlive = false;
        }

        private void ReloadData()
        {
            // if date and time difference from the last time is more than 60 seconds
            if ((DateTime.Now - lastRefreshTime).TotalSeconds >= 30)
            {
                // redo all the data
                data.outsideTemp = (float)GetRandomValue(58.44, (91 - 58.44) / 6);
                data.dewPoint = (float)GetRandomValue(48.43, (69 - 48.43) / 6);
                data.outsideHumidity = (int)GetRandomValue(50, (100 - 50) / 6);
                data.totalRain = (float)GetRandomValue(42, (54 - 42) / 6);
                data.dailyRain = (float)GetRandomValue(3.5, (4.5 - 3.5) / 6);
                data.monthlyRain = (float)GetRandomValue(3.5, (4.5 - 3.5) / 6);
                data.rainRate = (float)GetRandomValue(0.2, (1 - 0.2) / 6);
                data.heatIndex = (int)GetRandomValue(74.6, (84.6 - 74.6) / 6);
                data.windSpeed = (float)GetRandomValue(MiscellaneousHardwareConstants.SIMULATION_WEATHER_STATION_AVERAGE_WIND_SPEED_MPH, MiscellaneousHardwareConstants.SIMULATION_WEATHER_STATION_MAXIMUM_ALLOWABLE_WIND_SPEED_MPH_STD_DEV);
                data.baromPressure = (float)GetRandomValue(30, (40 - 30) / 6);
                data.windChill = (float)GetRandomValue(30, (40 - 30) / 6);

                data.windDirection = windDirections[windDirectionCounter];
                if (++windDirectionCounter >= 16)
                {
                    windDirectionCounter = 0;
                }


                // change the successfull date and time
                lastRefreshTime = DateTime.Now;
            }
        }

        private double GetRandomValue(double averageValue, double standardDeviation)
        {
            // Generate a Gaussian random distribution through a Box-Muller transform
            double rand1 = 1 - Rand.NextDouble();
            double rand2 = 1 - Rand.NextDouble();

            double randStdNorm = Math.Sqrt(-2 * Math.Log(rand1)) * Math.Sin(2 * Math.PI * rand2);
            double randNormValue = averageValue + (standardDeviation * randStdNorm);

            if (randNormValue < 0)
            {
                randNormValue = 0.0;
            }

            return randNormValue;
        }

        protected override bool TestIfComponentIsAlive()
        {
            return true;
        }

        public override float GetBarometricPressure()
        {
            return data.baromPressure;
        }

        public override float GetOutsideTemp()
        {
            return data.outsideTemp;
        }

        public override float GetInsideTemp()
        {
            return data.insideTemp;
        }

        public override float GetDewPoint()
        {
            return data.dewPoint;
        }

        public override float GetWindChill()
        {
            return data.windChill;
        }

        public override int GetHumidity()
        {
            return (int)data.outsideHumidity;
        }

        public override float GetTotalRain()
        {
            return data.totalRain;
        }

        public override float GetDailyRain()
        {
            return data.dailyRain;
        }

        public override float GetMonthlyRain()
        {
            return data.monthlyRain;
        }

        public override float GetWindSpeed()
        {
            // we call reload data here because this is the only function that is consistently called
            ReloadData();
            return data.windSpeed;
        }

        public override String GetWindDirection()
        {
            return data.windDirection;
        }

        public override float GetRainRate()
        {
            return data.rainRate;
        }

        public override int GetHeatIndex()
        {
            return (int)data.heatIndex;
        }
    }
}