using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMAutoNumber
{
    public class AutoNumberManager
    {
        public static int GetNextSequence(IOrganizationService service, string sequenceName)
        {
            int nextValue = 0;

            OrganizationServiceContext context = new OrganizationServiceContext(service);

            var sequence = (from seq in context.CreateQuery("ctccrm_autonumbersequence")
                            where seq.GetAttributeValue<string>("ctccrm_name") == sequenceName
                            select new
                            {
                                Id = seq.Id,
                                Value = seq.GetAttributeValue<int>("ctccrm_currentvalue")
                            }).FirstOrDefault();

            if (sequence == null)
                throw new InvalidPluginExecutionException("No record found for sequence named " + sequenceName);

            Entity seqUpdate = new Entity("ctccrm_autonumbersequence");
            seqUpdate.Id = sequence.Id;
            nextValue = sequence.Value +1;
            seqUpdate["ctccrm_currentvalue"] = nextValue;

            service.Update(seqUpdate);

            return nextValue;

        }
    }
}
