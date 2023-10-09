using System;
using Automatica.Core.Base.Templates;
using Automatica.Core.Driver;
using Automatica.Core.EF.Models;
using NodeDataType = Automatica.Core.Base.Templates.NodeDataType;

namespace Automatica.Driver.ShellyFactory
{
    public class ShellyFactory : DriverFactory
    {
        public static Guid DriverId = new Guid("c3cfe3fc-4440-4f8a-939a-5bdaab7f5c07");
        public static Guid Shelly1Device = new Guid("d5f3a2d0-275e-4cd3-86f8-cd358162fdff");
        public static Guid Shelly25Device = new Guid("46e1dbd5-45a2-4ed1-a6d0-d967f65579b6");
        public override string DriverName => "Shelly";

        public override Guid DriverGuid => DriverId;

        public override string ImageName => "automaticacore/plugin-automatica.driver.shelly";

        public override Version DriverVersion => new Version(0, 0, 0, 4);

        public override bool InDevelopmentMode => true;


        public const string DeviceIdPropertyKey = "shelly-id";

        public override IDriver CreateDriver(IDriverContext config)
        {
            return new ShellyDriver(config);
        }

        public override void InitNodeTemplates(INodeTemplateFactory factory)
        {
            factory.CreateInterfaceType(DriverGuid, "SHELLY.NAME", "SHELLY.DESCRIPTION", int.MaxValue, 1, true);
            factory.CreateNodeTemplate(DriverGuid, "SHELLY.NAME", "SHELLY.DESCRIPTION", "SHELLY", GuidTemplateTypeAttribute.GetFromEnum(InterfaceTypeEnum.Ethernet),
                DriverGuid, false, true, true, false, true, NodeDataType.NoAttribute, Int32.MaxValue, false);

            factory.CreatePropertyTemplate(new Guid("b6b5e4ff-8511-44f4-aceb-bd80d01a141b"), "SHELLY.SCAN.NAME", "SHELLY.SCAN.DESCRIPTION",
                "scan", PropertyTemplateType.Scan, DriverGuid, "COMMON.CATEGORY.DISCOVERY", true, false, "", null, 0,
                0);


            InitShelly1Templates(factory);
            InitShelly25Templates(factory);
        }

        private void InitShelly1Templates(INodeTemplateFactory factory)
        {
            var guid = Shelly1Device;
            factory.CreateInterfaceType(guid, "SHELLY.1.NAME", "SHELLY.1.DESCRIPTION", int.MaxValue, int.MaxValue, true);
            factory.CreateNodeTemplate(guid, "SHELLY.1.NAME", "SHELLY.1.DESCRIPTION", "shelly-1", DriverGuid,
                guid, false, true, true, false, true, NodeDataType.NoAttribute, Int32.MaxValue, false);

            var shelly1State = new Guid("05d9c7b0-dc49-4670-908a-09e792b0101a");
            factory.CreateNodeTemplate(shelly1State, "SHELLY.1.STATE.NAME", "SHELLY.1.STATE.DESCRIPTION", "state", guid,
                GuidTemplateTypeAttribute.GetFromEnum(InterfaceTypeEnum.Value), true, true, true, true, false, NodeDataType.Boolean, 1, false);
            factory.CreatePropertyTemplate(GenerateNewGuid(shelly1State, 1), "SHELLY.1.RELAY_CHANNEL",
                "SHELLY.1.RELAY_CHANNEL", "shelly-relay-channel", PropertyTemplateType.Numeric, shelly1State, "COMMON.CATEGORY.ADDRESS", false, false, null, 0, 0, 0);

            InitShellyParams(factory, guid);
        }

        private void InitShelly25Templates(INodeTemplateFactory factory)
        {
            var guid = Shelly25Device;
            factory.CreateInterfaceType(guid, "SHELLY.25.NAME", "SHELLY.25.DESCRIPTION", int.MaxValue, int.MaxValue, true);
            factory.CreateNodeTemplate(guid, "SHELLY.25.NAME", "SHELLY.25.DESCRIPTION", "shelly-25", DriverGuid,
                guid, false, true, true, false, true, NodeDataType.NoAttribute, Int32.MaxValue, false);

            var shelly25_0State = new Guid("144fbd35-fe2c-4b49-b0ec-23d87011291d");
            factory.CreateNodeTemplate(shelly25_0State, "SHELLY.25.STATE_1.NAME", "SHELLY.25.STATE_1.DESCRIPTION", "state", guid,
                GuidTemplateTypeAttribute.GetFromEnum(InterfaceTypeEnum.Value), true, true, true, true, false, NodeDataType.Boolean, 1, false);
            factory.CreatePropertyTemplate(GenerateNewGuid(shelly25_0State, 1), "SHELLY.25.RELAY_CHANNEL",
                 "SHELLY.25.RELAY_CHANNEL", "shelly-relay-channel", PropertyTemplateType.Numeric, shelly25_0State, "COMMON.CATEGORY.ADDRESS", false, false, null, 0, 0, 0);

            var shelly25_1State = new Guid("bd98cd45-c106-4f5f-832c-a246b8881ad0");
            factory.CreateNodeTemplate(shelly25_1State, "SHELLY.25.STATE_2.NAME", "SHELLY.25.STATE_2.DESCRIPTION", "state", guid,
                GuidTemplateTypeAttribute.GetFromEnum(InterfaceTypeEnum.Value), true, true, true, true, false, NodeDataType.Boolean, 1, false);
            factory.CreatePropertyTemplate(GenerateNewGuid(shelly25_1State, 1), "SHELLY.25.RELAY_CHANNEL_1",
                "SHELLY.25.RELAY_CHANNEL_1", "shelly-relay-channel", PropertyTemplateType.Numeric, shelly25_1State, "COMMON.CATEGORY.ADDRESS", false, false, null, 1, 0, 0);

            InitShellyParams(factory, guid);
        }


        private void InitShellyParams(INodeTemplateFactory factory, Guid parentGuid)
        {
            factory.CreatePropertyTemplate(GenerateNewGuid(parentGuid, 4), "SHELLY.PROPERTIES.POLLING_INTERVAL.NAME",
                "SHELLY.PROPERTIES.POLLING_INTERVAL.DESCRIPTION", "polling-interval", PropertyTemplateType.Integer, parentGuid, "COMMON.CATEGORY.ADDRESS", true, false, null, 2000, 0, 0);

            factory.CreatePropertyTemplate(GenerateNewGuid(parentGuid, 1), "SHELLY.PROPERTIES.ID.NAME",
                "SHELLY.PROPERTIES.ID.DESCRIPTION", DeviceIdPropertyKey, PropertyTemplateType.Text, parentGuid, "COMMON.CATEGORY.ADDRESS", true, false, null, 0, 0, 1);

            factory.CreatePropertyTemplate(GenerateNewGuid(parentGuid, 2), "SHELLY.PROPERTIES.USERNAME.NAME",
                "SHELLY.PROPERTIES.USERNAME.DESCRIPTION", "shelly-username", PropertyTemplateType.Text, parentGuid, "COMMON.CATEGORY.ADDRESS", true, false, null, "", 2, 3);
            factory.CreatePropertyTemplate(GenerateNewGuid(parentGuid, 3), "SHELLY.PROPERTIES.PASSWORD.NAME",
                "SHELLY.PROPERTIES.PASSWORD.DESCRIPTION", "shelly-password", PropertyTemplateType.Password, parentGuid, "COMMON.CATEGORY.ADDRESS", true, false, null, "", 0, 4);
        }

        private Guid GenerateNewGuid(Guid guid, int c)
        {
            byte[] gu = guid.ToByteArray();

            gu[^1] = (byte)(Convert.ToInt32(gu[^1]) + c);

            return new Guid(gu);
        }
    }
}
