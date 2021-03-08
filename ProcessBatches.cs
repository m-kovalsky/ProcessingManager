// Initial Parameter
var batchName = System.Environment.GetEnvironmentVariable("batchName");
bool runTMSL = true;

// Set parameters
string batchPrefix = "TabularProcessingBatch_";
bool seqEnabled = false;
int maxP = 0;
string batchNameFull = batchPrefix+batchName;
string processingMethod = string.Empty; // Database, Table, Partition
string[] typeList = {"full","automatic","calculate","clearValues","defragment","dataOnly"};
string processingType;
string timeSpent = "";

// Error check: Batch name
if (!Model.HasAnnotation(batchNameFull))
{
    Error("Invalid batch name. Please enter a valid batch name.");
    return;
}

string ann = Model.GetAnnotation(batchNameFull);

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
if (! typeList.Contains(processingType))
{
    Error("Invalid processing type. Ensure that the processing type is valid.");
    return;
}

// Additional set parameters
string databaseName = Model.Database.Name;
string newline = Environment.NewLine;
string seqStart = "{"+newline+"   \"sequence\":"+newline+"    {"+newline+"    \"maxParallelism\": "+maxP.ToString()+",   "+newline+"    \"operations\": ["+newline;
string tmslStart = "{" + newline + "  \"refresh\": {" + newline + "    \"type\": \"" + processingType + "\"," + newline + "    \"objects\": [ ";
string tmslMid = "{\"database\": \"" + databaseName + "\",\"table\": \"%table%\"} ";
string tmslMidDB = "{\"database\": \"" + databaseName + "\"}";
string tmslMidPart = "{\"database\": \"" + databaseName + "\",\"table\": \"%table%\" ,\"partition\": \"%partition%\"} ";
string tmslEnd = "    ]" + newline + "  }" + newline + "}";
string seqEnd = newline+"     ]"+newline+"   }"+newline+"}";
string tmsl = tmslStart;
string infoStart = "Processing type '"+processingType+"' of the '"+databaseName+"' model ";
string infoMid = "";
string info = string.Empty;
string infoEnd = " has finished in ";
var sw = new System.Diagnostics.Stopwatch();

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

// Generate TMSL
if (processingMethod == "Database")
{
    tmsl = tmsl + newline + "      " + tmslMidDB;
}
else if (processingMethod == "Table")
{
    int i=0;
    infoMid = "for the following tables: [";
    foreach (var t in Model.Tables.Where(a => a.HasAnnotation(batchNameFull) && a.GetAnnotation(batchNameFull) == "1"))
    {
        string tableName = t.Name;

        if (i == 0)
        {
            tmsl = tmsl + newline + "       " + tmslMid.Replace("%table%",tableName);
            infoMid = infoMid + "'"+tableName+"'";
        }
        else
        {
            tmsl = tmsl + newline + "      ," + tmslMid.Replace("%table%",tableName);
            infoMid = infoMid + ",'"+tableName+"'";
        }

        i++;        
    }

    infoMid = infoMid + "]";
}
else if (processingMethod == "Partition")
{
    int i=0;
    infoMid = "for the following partitions: [";
    foreach (var t in Model.Tables.Where(a => a.HasAnnotation(batchNameFull) || a.Partitions.Any(b => b.HasAnnotation(batchNameFull))))
    {
        string tableName = t.Name;
        if (t.HasAnnotation(batchNameFull))
        {
            foreach (var p in t.Partitions.ToList())
            {
                string pName = p.Name;

                if (i == 0)
                {
                    tmsl = tmsl + newline + "       " + tmslMidPart.Replace("%table%",tableName).Replace("%partition%",pName);
                    infoMid = infoMid + "'"+tableName+"'["+pName+"]";
                }
                else
                {
                    tmsl = tmsl + newline + "      ," + tmslMidPart.Replace("%table%",tableName).Replace("%partition%",pName);
                    infoMid = infoMid + ",'"+tableName+"'["+pName+"]";
                }

                i++;
            }
        }
        else
        {
            foreach (var p in t.Partitions.Where(a => a.HasAnnotation(batchNameFull)))
            {
                string pName = p.Name;

                if (i == 0)
                {
                    tmsl = tmsl + newline + "       " + tmslMidPart.Replace("%table%",tableName).Replace("%partition%",pName);
                    infoMid = infoMid + "'"+tableName+"'["+pName+"]";
                }
                else
                {
                    tmsl = tmsl + newline + "      ," + tmslMidPart.Replace("%table%",tableName).Replace("%partition%",pName);
                    infoMid = infoMid + ",'"+tableName+"'["+pName+"]";
                }

                i++;
            }
        }
    }

    infoMid = infoMid + "]";
}

info = infoStart + infoMid + infoEnd;
tmsl = tmsl + newline + tmslEnd;

// Add sequence if it is enabled
if (seqEnabled)
{
    tmsl = seqStart + tmsl + seqEnd;
}

if (runTMSL)
{
    sw.Start();
    ExecuteCommand(tmsl);
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

    Info(info + timeSpent);
    return;
}
else
{
    tmsl.Output();
}
