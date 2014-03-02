// <copyright file="AutoNumber.cs" company="">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author></author>
// <date>2/23/2014 10:43:52 AM</date>
// <summary>Implements the AutoNumber Workflow Activity.</summary>
namespace CRMAutoNumber
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    public sealed class AutoNumber : CodeActivity
    {
        [Input("Sequence Name")]
        public InArgument<string> SequenceName { get; set; }

        [Output("NextValue")]
        public OutArgument<int> NextValue { get; set; }

        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            // Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered AutoNumber.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            tracingService.Trace("AutoNumber.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var sequenceName = this.SequenceName.Get(executionContext);
                AutoNumberManager anm = new AutoNumberManager();
                var nextValue = anm.GetNextSequence(service, sequenceName);
                this.NextValue.Set(executionContext, nextValue);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());

                // Handle the exception.
                throw;
            }

            tracingService.Trace("Exiting AutoNumber.Execute(), Correlation Id: {0}", context.CorrelationId);
        }
    }
}