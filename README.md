# Processing Manager

Processing Manager is a tool that allows you to set customized processing 'batches' for a tabular model. This tool runs inside of [Tabular Editor](https://tabulareditor.com/ "Tabular Editor")'s [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window. This tool is compatible for all incarnations of tabular - [SQL Server Analysis Services](https://docs.microsoft.com/analysis-services/ssas-overview?view=asallproducts-allversions "SQL Server Analysis Services"), [Azure Analyis Services](https://azure.microsoft.com/services/analysis-services/ "Azure Analysis Services"), and [Power BI Premium](https://powerbi.microsoft.com/power-bi-premium/ "Power BI Premium") (using the [XMLA R/W Endpoint](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoint")).

## Purpose

This tool is designed to simplify the management of processing large tabular models.

## Running the Tool

To use the tool, download the ProcessingManager.cs script, paste it into the [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window in [Tabular Editor](https://tabulareditor.com/ "Tabular Editor") and click the play button (or press F5).  The tool itself does not process the model. It simply saves the instructions for processing the batches as metadata (annotations) within the model.

*Note: For easier access, it is recommended to save the script as a [Custom Action](https://docs.tabulareditor.com/Custom-Actions.html "Custom Action").*

## Processing Batches

After you set up a batch using the Processing Manager tool and deploy your model to a server, you can process the batch using this method.

1. Download the ProcessBatches.cs script and save it to your computer.
2. Run the code in the command prompt below (filling in the \<parameters\>) according to the variety of tabular you are using.

*Note: Please replace the 'batchName' parameter in each of the variations below with the name of the batch you would like to process.*

## [SQL Server Analysis Services](https://docs.microsoft.com/analysis-services/ssas-overview?view=asallproducts-allversions "SQL Server Analysis Services")

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "<Server Name>" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Azure Analyis Services](https://azure.microsoft.com/services/analysis-services/ "Azure Analysis Services")

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=asazure://<AAS Region>.asazure.windows.net/<AAS Server Name>;User ID=<xxxxx>;Password=<xxxxx>;Persist Security Info=True;Impersonation Level=Impersonate" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Power BI Premium](https://powerbi.microsoft.com/power-bi-premium/ "Power BI Premium")

Running this in Power BI Premium requires enabling [XMLA R/W Endpoints](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoints") for your Premium Workspace. An additional requirement is setting up a [Service Principal](https://tabulareditor.com/2020/06/02/PBI-SP-Access.html "Setting up a Service Principal").

    set batchName=batchName
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=powerbi://api.powerbi.com/v1.0/myorg/<Premium Workspace>;User ID=app:<Application ID>@<Tenant ID>;Password=<Application Secret>" "<Premium Dataset>" -S "<C# Script File Location (ProcessBatches.cs)>" 

## Additional Features

* Clicking the 'Sequence' check box will enable the [Sequence command](https://docs.microsoft.com/analysis-services/tmsl/sequence-command-tmsl?view=asallproducts-allversions "Sequence command") and allow you to set the [Max Parallelism](https://docs.microsoft.com/analysis-services/tmsl/sequence-command-tmsl?view=asallproducts-allversions#request "Max Parallelism") property.
* Export Script: When in the Summary view of the Processing Manager tool (the last window), you will see the Script button. Clicking it will dynamically generate a C# script which can be used to recreate the selected processing batch (by running the script in the [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window in [Tabular Editor](https://tabulareditor.com/ "Tabular Editor"). The script is automatically saved as a .cs file to your desktop. This may come in handy in scenarios where you need to copy batch modifications to different versions of a model (i.e. between branches or servers).

## Integration Applications

The command line code may be integrated into any application which is able to run command line code. Examples of such applications include [Azure DevOps](https://azure.microsoft.com/services/devops/ "Azure DevOps") and [Azure Data Factory](https://azure.microsoft.com/services/data-factory/ "Azure Data Factory"). Integrating the Processing Manager solution into these applciations will streamline the processing operations of your tabular model(s).
