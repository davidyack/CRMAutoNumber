using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace CRMAutoNumber.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAutoNumber()
        {
            CrmConnection c = new CrmConnection("CRM");
            var service = new OrganizationService(c);
            Entity seq = new Entity("ctccrm_autonumbersequence");
            var seqName   = Guid.NewGuid().ToString();
            seq["ctccrm_name"]= seqName;
            seq["ctccrm_currentvalue"] = 0;

            var seqID = service.Create(seq);

            int nextValue = 0;

            while (nextValue < 10)
            {
                AutoNumberManager anm = new AutoNumberManager();
                nextValue = anm.GetNextSequence(service, seqName);
            }

            service.Delete("ctccrm_autonumbersequence", seqID);
        }
    }
}
