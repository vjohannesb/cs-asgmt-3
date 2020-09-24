using Microsoft.Azure.Devices.Client;
using SharedLibrary.Services;
using System;
using SharedLibrary;

namespace ConsoleApp
{
    class Program
    {
        private static readonly DeviceClient deviceClient =
            DeviceClient.CreateFromConnectionString(Config.IotDeviceConnectionString, TransportType.Mqtt);

        static void Main(string[] args)
        {
            // Device Client (this) -> IoT Device
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();

            // Azure Function -> Device Client (this)
            DeviceService.ReceiveMessageAsync(deviceClient).GetAwaiter();

            // Pause
            Console.ReadKey();
        }
    }
}
