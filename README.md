CRM Auto Number for Microsoft Dynamics CRM
=============

Simple example of using a Real Time Workflow and a Custom Activty to Auto Number


Try it without building the code...simply import ctccrm_AutoNumberSequence_1_0_0_0_managed into your CRM organization

Remember to try things in a test organization (running with sharp objects can cause injury also FYI...)

Setup to Auto Number Account on the built-in Account Number field

1) Import Auto Number Solution (make sure to refresh to pickup navigation change)

2) Navigate to Settings -> Auto Number Sequences

3) Add a record using Name = Account and Current Value = 0

4) Create a Real Time Workflow against the Account Entity

5) Insert a step into the workflow from Utilities->Auto Number

6) Edit the properties on the Auto Number step and set Sequence Name = Account

7) Insert an Update Record step following the Auto Number Step

8) Set the properties Account Number to the Next Value of the Auto Number Step (Use the Form Assistant Local Values)



You could do the same thing on any other entity, you would just have to add a field to store the "number"
