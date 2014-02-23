CRMAutoNumber
=============

Simple example of using a Real Time Workflow and a Custom Activty to Auto Number


Try it without building the code...simply import ctccrm_AutoNumberSequence_1_0_0_0_managed into your CRM organization

Setup to Auto Number Account
1) Import Auto Number Solution
2) Navigate to Settings -> Auto Number Sequences
3) Add a record using Name = Account and Current Value = 0
4) Create a Real Time Workflow against the Account Entity
5) Insert a step into the workflow from Utilities->Auto Number
6) Edit the properties on the Auto Number step and set Sequence Name = Account
7) Insert an Update step following the Auto Number Step
8) Set the properties Account Number to the Next Value of the Auto Number Step 


You could do the same thing on any other entity, you would just have to add a field to store the "number"
