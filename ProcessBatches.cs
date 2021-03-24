#r "Microsoft.AnalysisServices.Core.dll"

// Initial Parameters
var batchName = System.Environment.GetEnvironmentVariable("batchName");
bool runTMSL = true;
bool seqEnabled = false;
int maxP = 0;

// Set parameters
string batchPrefix = "TabularProcessingBatch_";
string batchNameFull = batchPrefix+batchName;
string ann = Model.GetAnnotation(batchNameFull);
string processingMethod = string.Empty; // Database, Table, Partition
string[] typeList = {"full","automatic","calculate","clearvalues","defragment","dataonly"};
string processingType;
string timeSpent = "";
string databaseName = Model.Database.Name;
string newline = Environment.NewLine;
TOM.SaveOptions so = new TOM.SaveOptions();
var sw = new System.Diagnostics.Stopwatch();

// Error check: Batch name
if (!Model.HasAnnotation(batchNameFull))
{
    Error("Invalid batch name. Please enter a valid batch name.");
    return;
}

// Determine processing type, if sequence command is used, max parallelism
if (ann.Contains("_"))
{
    processingType = ann.Substring(0,ann.IndexOf("_"));
    seqEnabled = true;
    string maxPStr = ann.Substring(ann.IndexOf("_")+1);

    try
    {
        maxP = Convert.ToInt32(maxPStr);
    }
    catch
    {
        Error("Max Parallelism must be an integer.");
        return;
    }    
}
else
{
    processingType = ann;
}

// Error check: Processing type
if (! typeList.Contains(processingType.ToLower()))
{
    Error("Invalid processing type. Ensure that the processing type is valid.");
    return;
}

// Determine refresh type
var refType = TOM.RefreshType.Full;

if (processingType.ToLower() == "automatic")
{
    refType = TOM.RefreshType.Automatic;
}
else if (processingType.ToLower() == "dataonly")
{
    refType = TOM.RefreshType.DataOnly;
}
else if (processingType.ToLower() == "clearvalues")
{
    refType = TOM.RefreshType.ClearValues;
}
else if (processingType.ToLower() == "calculate")
{
    refType = TOM.RefreshType.Calculate;
}
else if (processingType.ToLower() == "defragment")
{
    refType = TOM.RefreshType.Defragment;
}

// Build Info output text
var sb_Info = new System.Text.StringBuilder();
sb_Info.Append("Processing type '"+processingType+"' of the '"+databaseName+"' model ");

// Identify processing method
if (Model.AllPartitions.Any(a => a.HasAnnotation(batchNameFull)))
{
    processingMethod = "Partition";
}
else if (Model.Tables.Any(a => a.HasAnnotation(batchNameFull)))
{
    processingMethod = "Table";
}
else
{
    processingMethod = "Database";
}

// Generate request refresh
if (processingMethod == "Database")
{
    Model.Database.TOMDatabase.Model.RequestRefresh(refType);
}
else if (processingMethod == "Table")
{    
    sb_Info.Append("for the following tables: [");
    foreach (var t in Model.Tables.Where(a => a.HasAnnotation(batchNameFull) && a.GetAnnotation(batchNameFull) == "1"))
    {
        string tableName = t.Name;
        Model.Database.TOMDatabase.Model.Tables[tableName].RequestRefresh(refType);   
        sb_Info.Append("'"+tableName+"',");
    }

    sb_Info.Remove(sb_Info.Length-1,1);
    sb_Info.Append("]");
}
else if (processingMethod == "Partition")
{
    sb_Info.Append("for the following partitions: [");    
    foreach (var t in Model.Tables.Where(a => a.HasAnnotation(batchNameFull) || a.Partitions.Any(b => b.HasAnnotation(batchNameFull))))
    {
        string tableName = t.Name;
        
        if (t.HasAnnotation(batchNameFull))
        {
            foreach (var p in t.Partitions.ToList())
            {
                string pName = p.Name;
                Model.Database.TOMDatabase.Model.Tables[tableName].Partitions[pName].RequestRefresh(refType);
                sb_Info.Append("'"+tableName+"'["+pName+"],");
            }
        }
        else
        {
            foreach (var p in t.Partitions.Where(a => a.HasAnnotation(batchNameFull)))
            {
                string pName = p.Name;
                Model.Database.TOMDatabase.Model.Tables[tableName].Partitions[pName].RequestRefresh(refType);
                sb_Info.Append("'"+tableName+"'["+pName+"],");             
            }
        }
    }

    sb_Info.Remove(sb_Info.Length-1,1);
    sb_Info.Append("]");
}

sb_Info.Append(" has finished in ");

// Execute refresh query
if (runTMSL)
{
    sw.Start();
    // Add sequence if it is enabled
    if (seqEnabled)
    {        
        so.MaxParallelism = maxP;        
    }
    Model.Database.TOMDatabase.Model.SaveChanges(so); 
    sw.Stop();    

    TimeSpan ts = sw.Elapsed;
  
    int sec = ts.Seconds;
    int min = ts.Minutes;
    int hr = ts.Hours;

    // Break down hours,minutes,seconds
    if (hr == 0)
    {
        if (min == 0)
        {
            timeSpent = sec + " seconds.";
        }
        else
        {
            timeSpent = min + " minutes and " + sec + " seconds.";
        }
    }
    else
    {
        timeSpent = hr + " hours, " + min + " minutes and " + sec + " seconds.";
    }

    if (hr == 1)
    {
        timeSpent = timeSpent.Replace("hours","hour");
    }
    if (min == 1)
    {
        timeSpent = timeSpent.Replace("minutes","minute");
    }
    if (sec == 1)
    {
        timeSpent = timeSpent.Replace("seconds","second");
    }

    Info(sb_Info.ToString() + timeSpent);
    return;
}
else
{
    if (processingMethod == "Database")
    {
        var x = Model.Database.TOMDatabase;
        TOM.JsonScripter.ScriptRefresh(x,refType).Output();
    }
    else if (processingMethod == "Table")
    {
        var x = Model.Database.TOMDatabase.Model.Tables.Where(a => a.Annotations.Where(b => b.Name == batchNameFull).Count() == 1).ToArray();
        TOM.JsonScripter.ScriptRefresh(x,refType).Output();
    }
    else if (processingMethod == "Partition")
    {
    }
}


