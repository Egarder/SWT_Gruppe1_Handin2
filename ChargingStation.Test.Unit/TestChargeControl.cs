using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStationClassLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace ChargingStation.Test.Unit
{
    [TestFixture]
    public class TestChargeControl
    {
        private IUsbCharger _usbChargerSource;
        private IChargeControl _uut;


        [SetUp]
        public void Setup()
        {
            _usbChargerSource = Substitute.For<IUsbCharger>();
            _uut = new ChargeControl(_usbChargerSource);
        }

        //Trods der ikke teste direkte på uut og at den kalder StopCharge, så 
        //raises der et event, som ChargeControl skal reagere på, hvis den reagerer på den
        //så vil den kalde sin StopCharge, som så kalder USBChargers stop charge, så nu 
        //testes den hele vejen ned fra event, chargecontrol og usbcharger, så fordi testen
        //godkendes er der den vej testet at det rigtige kaldes

        [Test]
        public void ChargeChanged_UnderFiveStopCharge_StopsCharge()
        {
            _usbChargerSource.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 2 });
            _usbChargerSource.Received(1).StopCharge();
        }
        [Test]
        public void ChargeChanged_OverFiveHundredStopCharge_StopsCharge()
        {
            _usbChargerSource.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = 505 });
            _usbChargerSource.Received(1).StopCharge();
        }
    }
}
