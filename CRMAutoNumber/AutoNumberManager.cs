using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
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

            var updaterTag = Guid.NewGuid().ToString();
            seqUpdate["ctccrm_updatertag"] = updaterTag;
            service.Update(seqUpdate);

            var seqVerify = service.Retrieve("ctccrm_autonumbersequence", seqUpdate.Id, 
                new ColumnSet(new string[] { "ctccrm_autonumbersequenceid", "ctccrm_updatertag", "ctccrm_currentvalue" }));

            if ((seqVerify.GetAttributeValue<string>("ctccrm_updatertag") != updaterTag) ||
                (seqVerify.GetAttributeValue<int>("ctccrm_currentvalue") != sequence.Value))
                throw new InvalidPluginExecutionException(OperationStatus.Retry,
                                  "Concurrency issue with next value- please retry");

            nextValue = sequence.Value +1;
            seqUpdate["ctccrm_currentvalue"] = nextValue;

            service.Update(seqUpdate);

            return nextValue;

        }
    }
}
