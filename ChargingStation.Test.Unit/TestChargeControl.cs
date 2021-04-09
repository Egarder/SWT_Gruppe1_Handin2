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
        //Her benyttes BVA til test
        [TestCase(4.999999)]
        [TestCase(5)]
        [TestCase(0.0000001)]
        [TestCase(-100000)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ChargeChanged_UnderFiveStopCharge_StopsCharge(double current)
        {
            _usbChargerSource.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = current });
            _usbChargerSource.Received(1).StopCharge();
        }
        [TestCase(500.001)]
        [TestCase(505)]
        [TestCase(10000000)]
        [TestCase(1000)]
        public void ChargeChanged_OverFiveHundredStopCharge_StopsCharge(double current)
        {
            _usbChargerSource.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = current });
            _usbChargerSource.Received(1).StopCharge();
        }


        [TestCase(5.0001)]
        [TestCase(10)]
        [TestCase(450)]
        [TestCase(499.999)]
        public void ChargeChanged_Over5Under500_StopChargeNotCalled(double current)
        {
            _usbChargerSource.ChargeEvent += Raise.EventWith(new ChargerEventArgs { Current = current });
            _usbChargerSource.Received(0).StopCharge();
        }

        [Test]
        public void StartCharge__chargerStartCharge_IsCalled()
        {
            _uut.StartCharge();
            _usbChargerSource.Received(1).StartCharge();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsConnected_ChargerConnectedTrue_ReturnsTrue(bool testBool)
        {
            _usbChargerSource.Connected = testBool;

            Assert.That(_uut.IsConnected,Is.EqualTo(testBool));
        }
    }
}
