# [Processing Manager](https://www.elegantbi.com/post/processingmanager "Processing Manager")

Make sure to read my [blog post](https://www.elegantbi.com/post/processingmanager "Processing Manager") on this tool!

Processing Manager is a tool that allows you to set customized processing 'batches' for a tabular model. This tool runs inside of [Tabular Editor](https://tabulareditor.com/ "Tabular Editor")'s [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window and is compatible for all incarnations of tabular - [SQL Server Analysis Services](https://docs.microsoft.com/analysis-services/ssas-overview?view=asallproducts-allversions "SQL Server Analysis Services"), [Azure Analysis Services](https://azure.microsoft.com/services/analysis-services/ "Azure Analysis Services"), and [Power BI Premium](https://powerbi.microsoft.com/power-bi-premium/ "Power BI Premium") (using the [XMLA R/W
endpoint](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoint")).

![Processing Manager](https://github.com/m-kovalsky/Tabular/blob/master/Icons/ProcessingManagerIcon.png "Processing Manager")


## Purpose

This tool is designed to simplify the management of processing large tabular models.

## Running the Tool

To use the tool, download the ProcessingManager.cs script, paste it into the [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window in [Tabular Editor](https://tabulareditor.com/ "Tabular Editor") and click the play button (or press F5).  The tool itself does not process the model. It simply saves the instructions for processing the batches as metadata (annotations) within the model.

Make sure to click the 'Save' button within the Processing Manager tool after making changes. If the 'Save' button is not clicked your changes will not be saved back to the model.

*Note: For easier access, it is recommended to save the script as a [Custom Action](https://docs.tabulareditor.com/Custom-Actions.html "Custom Action").*

*Note: If you are using the Processing Manager tool via a model created in Power BI Desktop (via opening Tabular Editor from the External Tools ribbon) you must ensure this setting is checked within Tabular Editor:*

    File -> Preferences -> Allow unsupported Power BI features (experimental)

## Processing the Batches

After you set up a batch using the Processing Manager tool and deploy your model to a server, you can process the batch using this method. 

1. Download the ProcessBatches.cs script and save it to your computer.
2. Run the code below in the command prompt (filling in the \<parameters\>) according to the variety of tabular you are using.

Since each of the scripts below uses an Environment Variable (set batchName=<Batch Name>), there is no need to copy the ProcessBatches.cs file for each batch. The same ProcessBatches.cs file can be referenced for processing all batches. Setting the 'batchName' Environment Variable instructs the code which batch to process.
    
***Note that although it is possible to run this in the command prompt, it is recommended to run the following processing scripts by integrating them into an application as discussed [below](https://github.com/m-kovalsky/ProcessingManager#integration-applications "Integration Applications").***

## [SQL Server Analysis Services](https://docs.microsoft.com/analysis-services/ssas-overview?view=asallproducts-allversions "SQL Server Analysis Services")

    set batchName=<Batch Name>
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "<Server Name>" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Azure Analysis Services](https://azure.microsoft.com/services/analysis-services/ "Azure Analysis Services")

    set batchName=<Batch Name>
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=asazure://<AAS Region>.asazure.windows.net/<AAS Server Name>;User ID=<xxxxx>;Password=<xxxxx>;Persist Security Info=True;Impersonation Level=Impersonate" "<Database Name>" -S "<C# Script File Location (ProcessBatches.cs)>"

## [Power BI Premium](https://powerbi.microsoft.com/power-bi-premium/ "Power BI Premium")

Running this in Power BI Premium requires enabling [XMLA R/W endpoints](https://docs.microsoft.com/power-bi/admin/service-premium-connect-tools "XMLA R/W Endpoints") for your Premium Workspace. An additional requirement is setting up a [Service Principal](https://tabulareditor.com/service-principal-access-to-dedicated-capacity-xmla-endpoint/ "Setting up a Service Principal").

    set batchName=<Batch Name>
    start /wait /d "C:\Program Files (x86)\Tabular Editor" TabularEditor.exe "Provider=MSOLAP;Data Source=powerbi://api.powerbi.com/v1.0/myorg/<Premium Workspace>;User ID=app:<Application ID>@<Tenant ID>;Password=<Application Secret>" "<Premium Dataset>" -S "<C# Script File Location (ProcessBatches.cs)>" 

## Additional Features

* Clicking the 'Sequence' check box will enable the [Sequence command](https://docs.microsoft.com/analysis-services/tmsl/sequence-command-tmsl?view=asallproducts-allversions "Sequence command") and allow you to set the [Max Parallelism](https://docs.microsoft.com/analysis-services/tmsl/sequence-command-tmsl?view=asallproducts-allversions#request "Max Parallelism") property (if using the Sequence command, the Max Parallelism value must be greater than 0).
* Export Script: When in the Summary view of the Processing Manager tool (the last window), you will see the Script button. Clicking it will dynamically generate a C# script which can be used to recreate the selected processing batch (by running the script in the [Advanced Scripting](https://docs.tabulareditor.com/Advanced-Scripting.html "Advanced Scripting") window in [Tabular Editor](https://tabulareditor.com/ "Tabular Editor")). The script is automatically saved as a .cs file to your desktop. This may come in handy in scenarios where you need to copy batch modifications to different versions of a model (i.e. between branches or servers).

## Integration Applications

The command line code may be integrated into any application which is able to run command line code. Examples of such applications include [Azure DevOps](https://azure.microsoft.com/services/devops/ "Azure DevOps") and [Azure Data Factory](https://azure.microsoft.com/services/data-factory/ "Azure Data Factory"). Integrating the Processing Manager solution into these applciations will streamline the processing operations of your tabular model(s). In order to use these applications for a Power BI Premium dataset you will need to set up a [Service Principal](https://tabulareditor.com/2020/06/02/PBI-SP-Access.html "Service Principal") and a [Key Vault](https://azure.microsoft.com/services/key-vault/ "Azure Key Vault"). 

Make sure to read my [blog post](https://www.elegantbi.com/post/processingmanager "Processing Manager") for more information on setting up the processing within [Azure DevOps](https://azure.microsoft.com/services/devops/ "Azure DevOps").

## Requirements

[Tabular Editor](https://tabulareditor.com/ "Tabular Editor") version 2.12.1 or higher.
