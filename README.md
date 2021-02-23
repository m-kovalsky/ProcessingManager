# Processing Manager

Processing Manager is a tool that allows you to set customized processing 'batches' for a tabular model. This tool runs inside of [Tabular Editor](https://tabulareditor.com/ "Tabular Editor")'s [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window. This tool is compatible for all incarnations of tabular - SQL Server Analysis Services, Azure Analysis Services, and Power BI Premium (using the [XMLA R/W Endpoint](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoint")).

## Purpose

This tool is designed to simplify the management of processing large tabular models.

## Running the Tool

To use the tool, simply download the ProcessingManager.cs script, paste it into the [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window in [Tabular Editor](https://tabulareditor.com/ "Tabular Editor") and click the play button (or press F5). The tool itself does not process the model. It simply saves the instructions for processing the batches as metadata (annotations) within the model.

## Processing Batches

After you set up a batch using the Processing Manager tool and deploy your model to a server, you can process the batch using this method.

1. Download the ProcessBatches.cs script and save it to your computer.
2. Run the code in the command prompt below (filling in the <parameters>) according to the variety of tabular you are using.

Note: Please replace the 'batchName' parameter in each of the variations below with the name of the batch you would like to process.

## [SQL Server Analysis Services](https://docs.microsoft.com/analysis-services/ssas-overview?view=asallproducts-allversions "SQL Server Analysis Services")

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "<Server Name>" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Azure Analyis Services](https://azure.microsoft.com/services/analysis-services/ "Azure Analysis Services")

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=asazure://<westeurope>.asazure.windows.net/<AASServerName>;User ID=<xxxxx>;Password=<xxxxx>;Persist Security Info=True;Impersonation Level=Impersonate" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Power BI Premium](https://powerbi.microsoft.com/power-bi-premium/ "Power BI Premium")

In order to run this for Power BI Premium, you will need to enable [XMLA R/W Endpoints](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoints") for your Premium Workspace. You will also need to set up a [Service Principal](https://tabulareditor.com/2020/06/02/PBI-SP-Access.html "Setting up a Service Principal").

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=powerbi://api.powerbi.com/v1.0/myorg/<Premium Workspace>;User ID=app:<Application ID>@<Tenant ID>;Password=<Application Secret>" "<Premium Dataset>" -S "<C# Script File Location (ProcessBatches.cs)>" 
    
    
