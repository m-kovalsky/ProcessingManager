#r "System.Drawing"

using System.Drawing;

// Parameters
int formWidth = 500;
int formHeight = 700;
string batchName = string.Empty;
string batchNameFull = string.Empty;
string processingType = string.Empty;
bool IsExpOrCol = false;
bool hasSaved = false;
bool seqEnabled = false;
bool darkModeEnabled = false;
int mp = 0;
string[] processingTypeList = {"automatic","full","calculate","clearValues","defragment","dataOnly"};
string[] infoList = {
	 "Select the tables/partitions to process in this batch."
	,"Here is a summary of the selected batch."
};
string batchPrefix = "TabularProcessingBatch_";
int batchPrefixLen = batchPrefix.Length;
var sb_ExportScript = new System.Text.StringBuilder();
string newline = Environment.NewLine;
string ebiURL = @"https://www.elegantbi.com";

// Start screen
System.Windows.Forms.Form newForm = new System.Windows.Forms.Form();
System.Windows.Forms.Panel startPanel = new System.Windows.Forms.Panel();
System.Windows.Forms.RadioButton newbatchButton = new System.Windows.Forms.RadioButton();
System.Windows.Forms.RadioButton existingbatchButton = new System.Windows.Forms.RadioButton();
System.Windows.Forms.ComboBox batchComboBox = new System.Windows.Forms.ComboBox();            
System.Windows.Forms.Button goButton = new System.Windows.Forms.Button();
System.Windows.Forms.Label homeToolLabel = new System.Windows.Forms.Label();
System.Windows.Forms.LinkLabel ebiHome = new System.Windows.Forms.LinkLabel();

// Main screen
System.Windows.Forms.Panel topPanel = new System.Windows.Forms.Panel();
System.Windows.Forms.Panel namePanel = new System.Windows.Forms.Panel();
System.Windows.Forms.Panel typePanel = new System.Windows.Forms.Panel();
System.Windows.Forms.Panel infoPanel = new System.Windows.Forms.Panel();
System.Windows.Forms.Panel treePanel = new System.Windows.Forms.Panel();
System.Net.WebClient w = new System.Net.WebClient();
System.Windows.Forms.TreeView treeView = new System.Windows.Forms.TreeView();
System.Windows.Forms.Label typeLabel = new System.Windows.Forms.Label();
System.Windows.Forms.ComboBox typeComboBox = new System.Windows.Forms.ComboBox();
System.Windows.Forms.CheckBox sequenceCheckBox = new System.Windows.Forms.CheckBox();
System.Windows.Forms.CheckBox summarySequenceCheckBox = new System.Windows.Forms.CheckBox();
System.Windows.Forms.TextBox batchNameTextBox = new System.Windows.Forms.TextBox();
System.Windows.Forms.ComboBox batchNameComboBox = new System.Windows.Forms.ComboBox();
System.Windows.Forms.Label nameLabel = new System.Windows.Forms.Label();
System.Windows.Forms.Label toolNameLabel = new System.Windows.Forms.Label();
System.Windows.Forms.Label infoLabel = new System.Windows.Forms.Label();
System.Windows.Forms.NumericUpDown maxPNumeric = new System.Windows.Forms.NumericUpDown();
System.Windows.Forms.NumericUpDown summaryMaxPNumeric = new System.Windows.Forms.NumericUpDown();
System.Windows.Forms.Label maxPLabel = new System.Windows.Forms.Label();
System.Windows.Forms.ToolTip maxPLabelToolTip = new System.Windows.Forms.ToolTip();
maxPLabelToolTip.SetToolTip(maxPLabel, "Specify the maximum number of threads used during processing. " + newline + "Valid values are any postive integer." + newline + "Setting the value to 1 equals not parallel (uses one thread).");
System.Windows.Forms.ImageList imageList = new System.Windows.Forms.ImageList();
System.Windows.Forms.ImageList imageList2 = new System.Windows.Forms.ImageList();
System.Windows.Forms.Button saveButton = new System.Windows.Forms.Button();
System.Windows.Forms.ToolTip saveButtonToolTip = new System.Windows.Forms.ToolTip();
saveButtonToolTip.SetToolTip(saveButton, "Saves the changes back to the model");
System.Windows.Forms.Button deleteButton = new System.Windows.Forms.Button();
System.Windows.Forms.ToolTip deleteButtonToolTip = new System.Windows.Forms.ToolTip();
deleteButtonToolTip.SetToolTip(deleteButton, "Deletes this batch");
System.Windows.Forms.Button summaryButton = new System.Windows.Forms.Button();
System.Windows.Forms.ToolTip summaryButtonToolTip = new System.Windows.Forms.ToolTip();
summaryButtonToolTip.SetToolTip(summaryButton, "Navigate to the summary page");
System.Windows.Forms.Button scriptButton = new System.Windows.Forms.Button();
System.Windows.Forms.ToolTip scriptButtonToolTip = new System.Windows.Forms.ToolTip();
scriptButtonToolTip.SetToolTip(scriptButton, "Saves a script to your desktop with the instructions to create the selected batch");
System.Windows.Forms.Button backButton = new System.Windows.Forms.Button();
System.Windows.Forms.ToolTip backButtonToolTip = new System.Windows.Forms.ToolTip();
backButtonToolTip.SetToolTip(backButton, "Navigate back the main page");
System.Windows.Forms.ToolTip sequenceCheckBoxToolTip = new System.Windows.Forms.ToolTip();
sequenceCheckBoxToolTip.SetToolTip(sequenceCheckBox, "Check this box to enable sequential processing");
System.Windows.Forms.Label saveComment = new System.Windows.Forms.Label();

// Summary Screen
System.Windows.Forms.TreeView summaryTreeView = new System.Windows.Forms.TreeView();
System.Windows.Forms.ComboBox summarytypeComboBox = new System.Windows.Forms.ComboBox();

// Colors
System.Drawing.Color sideColor =  ColorTranslator.FromHtml("#BFBFBF");
System.Drawing.Color bkgrdColor =  ColorTranslator.FromHtml("#F2F2F2");
System.Drawing.Color lighttextColor =  Color.White;
System.Drawing.Color darktextColor =  Color.Black;
System.Drawing.Color visibleColor = darktextColor;
System.Drawing.Color hiddenColor = Color.Gray;
System.Drawing.Color darkModeBack =  ColorTranslator.FromHtml("#444444");
System.Drawing.Color darkModeName =  ColorTranslator.FromHtml("#F2C811");
System.Drawing.Color darkModeText =  Color.White;

// Fonts
System.Drawing.Font toolNameFont = new Font("Century Gothic", 22);
System.Drawing.Font elegantFont = new Font("Century Gothic", 10, FontStyle.Italic);
System.Drawing.Font homeToolNameFont = new Font("Century Gothic", 24);
System.Drawing.Font stdFont = new Font("Century Gothic", 10);

// Form
newForm.TopLevel = true;
newForm.BackColor = bkgrdColor;
newForm.Text = "Processing Manager";
newForm.Size = new Size(formWidth,formHeight);
newForm.MaximumSize = new Size(formWidth,formHeight);
newForm.MinimumSize = new Size(formWidth,formHeight);
newForm.Controls.Add(startPanel);

newForm.Controls.Add(topPanel);
newForm.Controls.Add(namePanel);
newForm.Controls.Add(typePanel);
newForm.Controls.Add(infoPanel);
newForm.Controls.Add(treePanel);

int panelX = 0;

startPanel.Controls.Add(newbatchButton);
startPanel.Controls.Add(existingbatchButton);
startPanel.Controls.Add(batchComboBox);
startPanel.Controls.Add(goButton);
startPanel.Controls.Add(homeToolLabel);
startPanel.Visible = true;
startPanel.Size = new Size(formWidth,formHeight);
startPanel.Location = new Point(0,0);

startPanel.Controls.Add(ebiHome);
ebiHome.Text = "Designed by Elegant BI";
ebiHome.Size = new Size(200,40);
ebiHome.Location = new Point(165,460);
ebiHome.Font = elegantFont;

ebiHome.LinkClicked += (System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) => {

    System.Diagnostics.Process.Start(ebiURL);
};

// Panels
topPanel.Visible = false;
topPanel.Size = new Size(formWidth,70);
topPanel.Location = new Point(panelX,0);
topPanel.BackColor = bkgrdColor;
namePanel.Visible = false;
namePanel.Size = new Size(formWidth,30);
namePanel.Location = new Point(panelX,70);
namePanel.BackColor = bkgrdColor;
typePanel.Visible = false;
typePanel.Size = new Size(formWidth,30);
typePanel.Location = new Point(panelX,100);
typePanel.BackColor = bkgrdColor;
infoPanel.Visible = false;
infoPanel.Size = new Size(formWidth,30);
infoPanel.Location = new Point(panelX,130);
infoPanel.BackColor = bkgrdColor;
treePanel.Visible = false;
treePanel.Size = new Size(formWidth,500);
treePanel.Location = new Point(panelX,160);
treePanel.BackColor = bkgrdColor;

// Add items to panels
topPanel.Controls.Add(toolNameLabel);
topPanel.Controls.Add(saveButton);
topPanel.Controls.Add(deleteButton);
topPanel.Controls.Add(summaryButton);
topPanel.Controls.Add(scriptButton);
topPanel.Controls.Add(backButton);
namePanel.Controls.Add(nameLabel);
namePanel.Controls.Add(batchNameTextBox);
namePanel.Controls.Add(batchNameComboBox);
typePanel.Controls.Add(typeLabel);
typePanel.Controls.Add(typeComboBox);
typePanel.Controls.Add(summarytypeComboBox);
typePanel.Controls.Add(sequenceCheckBox);
typePanel.Controls.Add(summarySequenceCheckBox);
infoPanel.Controls.Add(infoLabel);
infoPanel.Controls.Add(maxPNumeric);
infoPanel.Controls.Add(summaryMaxPNumeric);
infoPanel.Controls.Add(maxPLabel);
treePanel.Controls.Add(treeView);
treePanel.Controls.Add(summaryTreeView);
treePanel.Controls.Add(saveComment);

// Start Screen Objects
homeToolLabel.Text = "Processing Manager";
homeToolLabel.Size = new Size(450,100);
homeToolLabel.Location = new Point(80,150);
homeToolLabel.Font = homeToolNameFont;


newbatchButton.Size = new Size(250,25);
newbatchButton.Location = new Point(130,250);
newbatchButton.Text = "Create New Processing Batch";
newbatchButton.Font = stdFont;

existingbatchButton.Size = new Size(250,25);
existingbatchButton.Location = new Point(130,280);
existingbatchButton.Text = "Modify Existing Processing Batch";
existingbatchButton.Font = stdFont;

batchComboBox.Visible = false;
batchComboBox.Size = new Size(180,20);
batchComboBox.Location = new Point(150,320);
batchComboBox.Font = stdFont;

goButton.Visible = false;
goButton.Size = new Size(100,30);
goButton.Text = "Go";
goButton.Font = stdFont;

int labelX = 10;
int labelY = 9;
// Main Screen Objects
toolNameLabel.Visible = true;
toolNameLabel.Size = new Size(330,50);
toolNameLabel.Location = new Point(5,15);
toolNameLabel.Text = "Processing Manager";
toolNameLabel.Font = toolNameFont;

nameLabel.Visible = true;
nameLabel.Size = new Size(70,20);
nameLabel.Location = new Point(labelX,labelY);
nameLabel.Text = "Batch Name:";

batchNameTextBox.Visible = true;
batchNameTextBox.Size = new Size(220,30);
batchNameTextBox.Location = new Point(80,5);
batchNameTextBox.Enabled = false;

batchNameComboBox.Visible = false;
batchNameComboBox.Size = new Size(220,30);
batchNameComboBox.Location = new Point(80,5);
batchNameComboBox.Enabled = true;

typeLabel.Visible = true;
typeLabel.Size = new Size(90,20);
typeLabel.Location = new Point(labelX,labelY);
typeLabel.Text = "Processing Type:";

typeComboBox.Visible = true;
typeComboBox.Size = new Size(200,20);
typeComboBox.Location = new Point(100,5);
typeComboBox.Enabled = true;

// Add items to processing type combo box
for (int i=0; i<processingTypeList.Count(); i++)
{
	typeComboBox.Items.Add(processingTypeList[i]);
	summarytypeComboBox.Items.Add(processingTypeList[i]);
}

int checkboxWidth = 90;
int gap = 10;

summarytypeComboBox.Visible = false;
summarytypeComboBox.Size = new Size(200,20);
summarytypeComboBox.Location = new Point(100,5);
summarytypeComboBox.Enabled = false;

sequenceCheckBox.Visible = true;
sequenceCheckBox.Size = new Size(checkboxWidth,20);
sequenceCheckBox.Location = new Point(formWidth-gap-checkboxWidth,7);
sequenceCheckBox.Checked = false;
sequenceCheckBox.Enabled = true;
sequenceCheckBox.Text = "Sequence";

summarySequenceCheckBox.Visible = false;
summarySequenceCheckBox.Size = new Size(checkboxWidth,20);
summarySequenceCheckBox.Location = new Point(formWidth-gap-checkboxWidth,7);
summarySequenceCheckBox.Checked = false;
summarySequenceCheckBox.Enabled = false;
summarySequenceCheckBox.Text = "Sequence";

maxPNumeric.Visible = false;
maxPNumeric.Size = new Size(45,20);
maxPNumeric.Location = new Point(345,5);

summaryMaxPNumeric.Visible = false;
summaryMaxPNumeric.Size = new Size(45,20);
summaryMaxPNumeric.Location = new Point(345,5);
summaryMaxPNumeric.Enabled = false;

maxPLabel.Visible = false;
maxPLabel.Size = new Size(90,20);
maxPLabel.Location = new Point(394,7);
maxPLabel.Text = "Max Parallelism";

infoLabel.Visible = true;
infoLabel.Size = new Size(300,30);
infoLabel.Location = new Point(labelX,labelY);
infoLabel.Text = infoList[0];

treeView.Visible = true;
treeView.Size = new Size(formWidth - 35,480);
treeView.Location = new Point(gap,gap);
treeView.StateImageList = new System.Windows.Forms.ImageList();
treeView.BackColor = Color.White;
treeView.CheckBoxes = false;

summaryTreeView.Visible = false;
summaryTreeView.Size = new Size(formWidth - 35,480);
summaryTreeView.Location = new Point(gap,gap);
summaryTreeView.StateImageList = new System.Windows.Forms.ImageList();
summaryTreeView.BackColor = bkgrdColor;
summaryTreeView.CheckBoxes = false;

// Add images from web to Image List
var urlPrefix = "https://raw.githubusercontent.com/m-kovalsky/Tabular/master/Icons/";
var urlSuffix = "Icon.png";

string[] imageURLList = { "Table", "Partition", "SummaryTable", "SummaryPartition", "Model", "SummaryModel","SaveDark", "Script", "Delete","ForwardArrow", "BackArrow"};
for (int i = 0; i < imageURLList.Count(); i++)
{
    var url = urlPrefix + imageURLList[i] + urlSuffix;      
    byte[] imageByte = w.DownloadData(url);
    System.IO.MemoryStream ms = new System.IO.MemoryStream(imageByte);
    System.Drawing.Image im = System.Drawing.Image.FromStream(ms);

    if (i<6)
    {
        imageList.Images.Add(im);
    }
    else
    {
        imageList2.Images.Add(im);
    }
}  

string[] stateimageURLList = { "Unchecked", "Checked", "PartiallyChecked" };
for (int i = 0; i < stateimageURLList.Count(); i++)
{
    var url = urlPrefix + stateimageURLList[i] + urlSuffix;      
    byte[] imageByte = w.DownloadData(url);
    System.IO.MemoryStream ms = new System.IO.MemoryStream(imageByte);
    System.Drawing.Image im = System.Drawing.Image.FromStream(ms);
    treeView.StateImageList.Images.Add(im);
}  

imageList.ImageSize = new Size(16, 16); 
treeView.ImageList = imageList;
summaryTreeView.ImageList = imageList;
imageList2.ImageSize = new Size(23, 23); 
saveButton.ImageList = imageList2;
deleteButton.ImageList = imageList2;
summaryButton.ImageList = imageList2;
scriptButton.ImageList = imageList2;
backButton.ImageList = imageList2;

// Lambda expression for all the steps to next page
System.Action<int> NextStep = stepNumber =>
{
	if (stepNumber == 0)
	{
		treeView.Nodes.Clear();
		// Add model node
		string modelName = Model.Database.Name;
		var mn = treeView.Nodes.Add(modelName);
		mn.ImageIndex = 4;
		mn.SelectedImageIndex = 4;
		mn.StateImageIndex = 0;

		foreach (var t in Model.Tables.OrderBy(a => a.Name).ToList())
		{  
		    // Add table nodes
		    string tableName = t.Name;    
		    var tn = mn.Nodes.Add(tableName);
		    tn.ImageIndex = 0;
		    tn.SelectedImageIndex = 0;
		    tn.StateImageIndex = 0;    

		    // Add partition sub-nodes
		    foreach (var p in t.Partitions.OrderBy(a => a.Name).ToList())
		    {
		        string pName = p.Name;
		        var x = tn.Nodes.Add(pName);  

	            x.ImageIndex = 1;
	            x.SelectedImageIndex = 1;
  		        x.StateImageIndex = 0;
		    }
		}
	}
	else if (stepNumber == 1)
	{
		treeView.Nodes.Clear();
		batchNameFull = batchPrefix+batchName;

		if (seqEnabled)
		{
			maxPNumeric.Visible = true;
			sequenceCheckBox.Checked = true;
		}
		else
		{
			maxPNumeric.Visible = false;
			sequenceCheckBox.Checked = false;
		}

		// Add model node
		string modelName = Model.Database.Name;
		var mn = treeView.Nodes.Add(modelName);
		mn.ImageIndex = 4;
		mn.SelectedImageIndex = 4;

		if (Model.Tables.Any(a => a.HasAnnotation(batchNameFull)) || Model.AllPartitions.Any(b => b.HasAnnotation(batchNameFull)))
		{
			mn.StateImageIndex = 2;
		}
		else
		{
			mn.StateImageIndex = 1;
		}		

		// Add nodes to treeview
		foreach (var t in Model.Tables.OrderBy(a => a.Name).ToList())
		{  
		    // Add table nodes
		    string tableName = t.Name;    
		    var tn = mn.Nodes.Add(tableName);
		    tn.ImageIndex = 0;
		    tn.SelectedImageIndex = 0;
		    tn.StateImageIndex = 0;   
		   
		    if (t.HasAnnotation(batchNameFull) || mn.StateImageIndex == 1)
		    {
		    	tn.StateImageIndex = 1;		    	
		    }
		    else if (t.Partitions.Any(b => b.HasAnnotation(batchNameFull)))
		    {
		    	tn.StateImageIndex = 2;
		    }

		    // Add partition sub-nodes
		    foreach (var p in t.Partitions.OrderBy(a => a.Name).ToList())
		    {
		        string pName = p.Name;
		        var x = tn.Nodes.Add(pName);  

	            x.ImageIndex = 1;
	            x.SelectedImageIndex = 1;
	            x.StateImageIndex = 0;

	            if (t.HasAnnotation(batchNameFull))
	            {
	            	x.StateImageIndex = 1;
	            }
	            else if (p.HasAnnotation(batchNameFull))
	            {
	            	x.StateImageIndex = 1;
	            }  
		    }
		}
	}
	else if (stepNumber == 2)
	{
		summaryTreeView.Nodes.Clear();
		batchNameFull = batchPrefix+batchName;

			sb_ExportScript.Clear();            
            sb_ExportScript.Append("string batchNameFull =\""+batchNameFull+"\";" + newline);
            sb_ExportScript.Append("string processingType =\""+processingType+"\";" + newline + newline);
            
            // Script: Remove existing annotations
            sb_ExportScript.Append("foreach (var t in Model.Tables.ToList())"+newline+"{"+newline+"    t.RemoveAnnotation(batchNameFull);"+newline+"}" + newline + newline);
            sb_ExportScript.Append("foreach (var p in Model.AllPartitions)"+newline+"{"+newline+"    p.RemoveAnnotation(batchNameFull);"+newline+"}" + newline + newline);

            // Script: Update model annotation
            if (summarySequenceCheckBox.Checked)
            {
            	sb_ExportScript.Append("Model.SetAnnotation(batchNameFull,processingType"+"+\"_"+Convert.ToInt32(summaryMaxPNumeric.Value).ToString()+"\");" + newline + newline);
            }
            else
            {
            	sb_ExportScript.Append("Model.SetAnnotation(batchNameFull,processingType);" + newline + newline);	
            }
            
        // Add model node
		string modelName = Model.Database.Name;
		var mn = summaryTreeView.Nodes.Add(modelName);
		mn.ImageIndex = 5;
		mn.SelectedImageIndex = 5;

		if (Model.Tables.Any(a => a.HasAnnotation(batchNameFull)) || Model.AllPartitions.Any(b => b.HasAnnotation(batchNameFull)))
		{
			mn.StateImageIndex = 2;
		}
		else
		{
			mn.StateImageIndex = 1;
		}		

		// Add nodes to treeview
		foreach (var t in Model.Tables.Where(a => a.HasAnnotation(batchNameFull) || a.Partitions.Any(b => b.HasAnnotation(batchNameFull))).OrderBy(a => a.Name).ToList())
		{  
		    // Add table nodes
		    string tableName = t.Name;    
		    var tn = mn.Nodes.Add(tableName);
		    tn.ImageIndex = 2;
		    tn.SelectedImageIndex = 2;
		   
		    if (t.HasAnnotation(batchNameFull))
		    {
		    	sb_ExportScript.Append("Model.Tables[\""+tableName+"\"].SetAnnotation(batchNameFull,\"1\");" + newline);		    
		    }
		    else if (t.Partitions.Any(b => b.HasAnnotation(batchNameFull)))
		    {
		    }   

		    // Add partition sub-nodes
		    foreach (var p in t.Partitions.Where(a => a.HasAnnotation(batchNameFull)).OrderBy(a => a.Name).ToList())
		    {
		        string pName = p.Name;
		        var x = tn.Nodes.Add(pName);  

	            x.ImageIndex = 3;
	            x.SelectedImageIndex = 3;

	            if (t.HasAnnotation(batchNameFull))
	            {
	            }
	            else if (p.HasAnnotation(batchNameFull))
	            {
	            	sb_ExportScript.Append("Model.Tables[\""+tableName+"\"].Partitions[\""+pName+"\"].SetAnnotation(batchNameFull,\"1\");" + newline);
	            }  
		    }
		}
	}
};

int goButtonX = 190;
int goButtonY = 330;
// Event Handlers (Start Screen)
newbatchButton.Click += (System.Object sender, System.EventArgs e) => {

    goButton.Visible = true;
    goButton.Location = new Point(goButtonX, goButtonY);
    batchComboBox.Visible = false;
    goButton.Enabled = true;
    batchComboBox.Text = string.Empty;
    batchNameTextBox.Enabled = true;
};

batchComboBox.SelectedValueChanged += (System.Object sender, System.EventArgs e) => {

    goButton.Enabled = true;         
};

existingbatchButton.Click += (System.Object sender, System.EventArgs e) => {

    goButton.Location = new Point(goButtonX, goButtonY+30);
    batchComboBox.Visible = true;
    goButton.Visible = true;    
    goButton.Enabled = false;
    batchNameTextBox.Enabled = false;
    
    // Populate Batch Combo Box
    batchComboBox.Items.Clear();
    foreach(var x in Model.GetAnnotations().Where(a => a.StartsWith(batchPrefix)))
    {    	
        batchComboBox.Items.Add(x.Substring(batchPrefixLen));    
    }
    
    if (batchComboBox.SelectedItem == null)
    {
        goButton.Enabled = false;
    }  
};

goButton.Click += (System.Object sender, System.EventArgs e) => {

    startPanel.Visible = false;
    topPanel.Visible = true;
    namePanel.Visible = true;
    typePanel.Visible = true;
    infoPanel.Visible = true;
    treePanel.Visible = true;
    
    if (existingbatchButton.Checked == true)
    {
        batchName = batchComboBox.Text;
        batchNameFull = batchPrefix+batchName;
        batchNameTextBox.Text = batchName;
        batchNameTextBox.Enabled = false;
        string ann = Model.GetAnnotation(batchNameFull);
        if (ann.Contains("_"))
        {
        	processingType = ann.Substring(0,ann.IndexOf("_"));
        	seqEnabled = true;
        	mp = Convert.ToInt32(ann.Substring(ann.IndexOf("_")+1));
        	maxPNumeric.Value = mp;
        }
        else
        {
        	processingType = ann;
        }
        typeComboBox.Text = processingType;

        NextStep(1); 
    }
    else
    {
       //batchName = batchNameTextBox.Text;
       NextStep(0); 
    }
};

treeView.AfterExpand += (System.Object sender, System.Windows.Forms.TreeViewEventArgs e) => {
    
    IsExpOrCol = true;
};

treeView.AfterCollapse += (System.Object sender, System.Windows.Forms.TreeViewEventArgs e) => {
    
    IsExpOrCol = true;
};

treeView.NodeMouseClick += (System.Object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e) => {
    
    if (IsExpOrCol == false)
    {
        if (e.Node.StateImageIndex != 1)
        {
            e.Node.StateImageIndex = 1;
        }
        else if (e.Node.StateImageIndex == 1)
        {
            e.Node.StateImageIndex = 0;
        }
        
        // Model
        if (e.Node.ImageIndex == 4)
        {
        	if (e.Node.StateImageIndex == 1)
        	{
        		foreach (System.Windows.Forms.TreeNode childNode in e.Node.Nodes)
        		{
        			childNode.StateImageIndex = 1;

        			foreach (System.Windows.Forms.TreeNode gChildNode in childNode.Nodes)
        			{
        				gChildNode.StateImageIndex = 1;
        			}
        		}
        	}
        	else if (e.Node.StateImageIndex == 0)
        	{
        		foreach (System.Windows.Forms.TreeNode childNode in e.Node.Nodes)
        		{
        			childNode.StateImageIndex = 0;

        			foreach (System.Windows.Forms.TreeNode gChildNode in childNode.Nodes)
        			{
        				gChildNode.StateImageIndex = 0;
        			}
        		}
        	}
        }

        if (e.Node.ImageIndex == 0)
        {
        	int tableNodeCount = e.Node.Parent.Nodes.Count;
        	int tableNodeCheckedCount = 0;
            // If parent node is checked, check all child nodes
            if (e.Node.StateImageIndex == 1)        
            {   
                foreach (System.Windows.Forms.TreeNode childNode in e.Node.Nodes)
                {
                    childNode.StateImageIndex = 1;
                    tableNodeCheckedCount++;               
                }               
            }
            // If parent node is unchecked, uncheck all child nodes
            else if (e.Node.StateImageIndex == 0)
            {
                foreach (System.Windows.Forms.TreeNode childNode in e.Node.Nodes)
                {
                    childNode.StateImageIndex = 0;                
                }
            }

            if (tableNodeCheckedCount == tableNodeCount)
            {
            	e.Node.Parent.StateImageIndex = 1;
            }
            else if (tableNodeCheckedCount > 0)
            {
            	e.Node.Parent.StateImageIndex = 2;
            }
        }     
        else if (e.Node.ImageIndex == 1)
        {
            int childNodeCount = e.Node.Parent.Nodes.Count;   
            int childNodeCheckedCount = 0;    
        
            foreach (System.Windows.Forms.TreeNode n in e.Node.Parent.Nodes)
            {
                if (n.StateImageIndex == 1)
                {
                    childNodeCheckedCount++;
                }
            }
            
            // If all child nodes are checked, set parent node to checked
            if (childNodeCheckedCount == childNodeCount)
            {
                e.Node.Parent.StateImageIndex = 1;
            }
            // If no child nodes are checked, set parent node to unchecked
            else if (childNodeCheckedCount == 0)
            {
                e.Node.Parent.StateImageIndex = 0;
            }
            // If not all children nodes are selected, set parent node to partially checked icon
            else if (childNodeCheckedCount < childNodeCount)
            {
                e.Node.Parent.StateImageIndex = 2;
            }
        }

        foreach (System.Windows.Forms.TreeNode modelNode in treeView.Nodes)
        {
        	int tableNodeCount = modelNode.Nodes.Count;
        	int tableNodeCheckedCount = 0;
            int partitionNodeCheckedCount = 0;
            foreach (System.Windows.Forms.TreeNode tableNode in modelNode.Nodes)
            {
            	if (tableNode.StateImageIndex == 1)
            	{
            		tableNodeCheckedCount++;
            	}
            	
            	foreach(System.Windows.Forms.TreeNode partitionNode in tableNode.Nodes)
            	{
            		if (partitionNode.StateImageIndex == 1)
            		{
            			partitionNodeCheckedCount++;
            		}
            	}
            }

            if (tableNodeCheckedCount == tableNodeCount)
            {
            	modelNode.StateImageIndex = 1;
            }
            else if (tableNodeCheckedCount == 0 && partitionNodeCheckedCount == 0)
            {
            	modelNode.StateImageIndex = 0;
            }
            else
            {
            	modelNode.StateImageIndex = 2;
            }
        }
    }

    IsExpOrCol = false;
};

// Top buttons
int saveButtonX = 385;
int buttonY = 25;
int buttonGap = 30;
int buttonSize = 25;

saveButton.Visible = true;
saveButton.ImageIndex = 0;
saveButton.Size = new Size(buttonSize,buttonSize);
saveButton.Location = new Point(saveButtonX,buttonY);
saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
saveButton.FlatAppearance.BorderSize = 0;
saveButton.TabStop = false;

deleteButton.Visible = true;
deleteButton.ImageIndex = 2;
deleteButton.Size = new Size(buttonSize,buttonSize);
deleteButton.Location = new Point(saveButtonX+(buttonGap*1),buttonY);
deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
deleteButton.FlatAppearance.BorderSize = 0;
deleteButton.TabStop = false;

summaryButton.Visible = true;
summaryButton.ImageIndex = 3;
summaryButton.Size = new Size(buttonSize,buttonSize);
summaryButton.Location = new Point(saveButtonX+(buttonGap*2),buttonY);
summaryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
summaryButton.FlatAppearance.BorderSize = 0;
summaryButton.TabStop = false;

scriptButton.Visible = false;
scriptButton.ImageIndex = 1;
scriptButton.Size = new Size(buttonSize,buttonSize);
scriptButton.Location = new Point(saveButtonX+(buttonGap*1),buttonY);
scriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
scriptButton.FlatAppearance.BorderSize = 0;
scriptButton.TabStop = false;

backButton.Visible = false;
backButton.ImageIndex = 4;
backButton.Size = new Size(buttonSize,buttonSize);
backButton.Location = new Point(saveButtonX+(buttonGap*2),buttonY);
backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
backButton.FlatAppearance.BorderSize = 0;
backButton.TabStop = false;

saveButton.MouseEnter += (System.Object sender, System.EventArgs e) => {
      
    saveButton.ImageIndex = 0;
    saveButton.FlatAppearance.BorderSize = 0;
};

saveButton.MouseLeave += (System.Object sender, System.EventArgs e) => {
      
    saveButton.ImageIndex = 0;
    saveButton.FlatAppearance.BorderSize = 0;
};

deleteButton.MouseEnter += (System.Object sender, System.EventArgs e) => {
      
    deleteButton.ImageIndex = 2;
    deleteButton.FlatAppearance.BorderSize = 0;
};

deleteButton.MouseLeave += (System.Object sender, System.EventArgs e) => {
      
    deleteButton.ImageIndex = 2;
    deleteButton.FlatAppearance.BorderSize = 0;
};

summaryButton.MouseEnter += (System.Object sender, System.EventArgs e) => {
      
    summaryButton.ImageIndex = 3;
    summaryButton.FlatAppearance.BorderSize = 0;
};

summaryButton.MouseLeave += (System.Object sender, System.EventArgs e) => {
      
    summaryButton.ImageIndex = 3;
    summaryButton.FlatAppearance.BorderSize = 0;
};

scriptButton.MouseEnter += (System.Object sender, System.EventArgs e) => {
      
    scriptButton.ImageIndex = 1;
    scriptButton.FlatAppearance.BorderSize = 0;
};

scriptButton.MouseLeave += (System.Object sender, System.EventArgs e) => {
      
    scriptButton.ImageIndex = 1;
    scriptButton.FlatAppearance.BorderSize = 0;
};

backButton.MouseEnter += (System.Object sender, System.EventArgs e) => {
      
    backButton.ImageIndex = 4;
    backButton.FlatAppearance.BorderSize = 0;
};

backButton.MouseLeave += (System.Object sender, System.EventArgs e) => {
      
    backButton.ImageIndex = 4;
    backButton.FlatAppearance.BorderSize = 0;
};

saveButton.Click += (System.Object sender, System.EventArgs e) => {

	batchName = batchNameTextBox.Text;
	processingType = typeComboBox.Text;
	
	if (sequenceCheckBox.Checked)
	{
		seqEnabled = true;
		mp = Convert.ToInt32(maxPNumeric.Value);
	}

	if (batchName.Length > 0)
	{
		batchNameFull = batchPrefix+batchName;	
	}

	int tableNodeSelCount = 0;
	int partitionNodeSelCount = 0;

	foreach (System.Windows.Forms.TreeNode modelNode in treeView.Nodes)
	{
		foreach (System.Windows.Forms.TreeNode tableNode in modelNode.Nodes)
		{
			if (tableNode.StateImageIndex == 1)
			{
				tableNodeSelCount++;
			}

			foreach (System.Windows.Forms.TreeNode partitionNode in tableNode.Nodes)
			{
				if (partitionNode.StateImageIndex == 1)
				{
					partitionNodeSelCount++;
				}
			}
		}
	}

	if (batchName.Length == 0)
	{
		Error("Batch not saved. Must enter a valid batch name.");
	}
	else if (processingType.Length == 0)
	{
		Error("Batch not saved. Must enter a valid processing type.");
	}
	else if (newbatchButton.Checked == true && Model.HasAnnotation(batchNameFull) && hasSaved == false)
	{
		Error("Batch not saved. Batch name cannot be the same as an existing batch name. Please enter a different batch name.");
	}
	else if (tableNodeSelCount == 0 && partitionNodeSelCount == 0)
	{
		Error("Batch not saved. Batch must have at least one table or partition selected.");
	}

	else
	{
		// Update Model annotation
		
		if (seqEnabled == true && mp == 0)
		{
			Error("Max Parallelism must be greater than 0");
			return;
		}
		else if (seqEnabled == true && mp > 0)
		{
			Model.SetAnnotation(batchNameFull,processingType+"_"+mp.ToString());
		}
		else
		{
			Model.SetAnnotation(batchNameFull,processingType);
		}

		foreach (System.Windows.Forms.TreeNode modelNode in treeView.Nodes)
        {
        	foreach (System.Windows.Forms.TreeNode rootNode in modelNode.Nodes)
	        {
	            string tableName = rootNode.Text;

	            // Update table annotations
	            if (rootNode.StateImageIndex == 1 && modelNode.StateImageIndex != 1)
	            {
	            	Model.Tables[tableName].SetAnnotation(batchNameFull,"1");

	            	foreach (var p in Model.AllPartitions.Where(a => a.Table.Name == tableName))
	            	{
	            		p.RemoveAnnotation(batchNameFull);
	            	}
	            }
	            else
	            {
	            	Model.Tables[tableName].RemoveAnnotation(batchNameFull);
	            }
	             
	            foreach (System.Windows.Forms.TreeNode childNode in rootNode.Nodes)
	            {
	                string partitionName = childNode.Text;
	                
	                // Update partition annotations
	                if (childNode.StateImageIndex == 1 && rootNode.StateImageIndex != 1)
	                {
	                	Model.Tables[tableName].Partitions[partitionName].SetAnnotation(batchNameFull,"1");
	                }
	                else if (childNode.StateImageIndex == 0)
	                {
	                	Model.Tables[tableName].Partitions[partitionName].RemoveAnnotation(batchNameFull);
	                }
	            }
	        }
        }

        hasSaved = true;
	}
};

deleteButton.Click += (System.Object sender, System.EventArgs e) => {

	if (batchName.Length == 0)
	{
		Warning("A valid batch name has not been entered. This batch has not been deleted.");
	}
	else
	{
		if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete the '"+batchName+"' batch?","Delete Batch",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
		{
			batchNameFull = batchPrefix+batchName;
			Model.RemoveAnnotation(batchNameFull);

			foreach (var t in Model.Tables.ToList())
			{
				t.RemoveAnnotation(batchNameFull);

				foreach( var p in t.Partitions.ToList())
				{
					p.RemoveAnnotation(batchNameFull);
				}
			}

			Info("Processing batch '"+batchName+"' has been deleted.");
		}
	}
};

summaryButton.Click += (System.Object sender, System.EventArgs e) => {

	if (! (hasSaved == true || existingbatchButton.Checked == true))
	{
		Error("Invalid batch. Please enter and save a valid batch.");
	}
	else
	{
		treeView.Visible = false;
		summaryTreeView.Visible = true;
		infoLabel.Text = infoList[1];
		batchNameTextBox.Visible = false;
		batchNameComboBox.Visible = true;
		batchNameComboBox.Text = batchName;
		batchNameFull = batchPrefix + batchName;
		typeComboBox.Visible = false;
		summarytypeComboBox.Visible = true;
		string ann = Model.GetAnnotation(batchNameFull);

		summaryButton.Visible = false;
		scriptButton.Visible = true;
		backButton.Visible = true;
		saveButton.Visible = false;
		deleteButton.Visible = false;
		sequenceCheckBox.Visible = false;
		summarySequenceCheckBox.Visible = true;
		maxPLabel.ForeColor = hiddenColor;
		maxPNumeric.Visible = false;

		if (seqEnabled)
		{
			summaryMaxPNumeric.Visible = true;
			summarytypeComboBox.Text = ann.Substring(0,ann.IndexOf("_"));
			summaryMaxPNumeric.Value = Convert.ToInt32(ann.Substring(ann.IndexOf("_")+1));
		}
		else
		{
			summarytypeComboBox.Text = ann;
		}

		batchNameComboBox.Items.Clear();
		// Populate batch name combo box
		foreach(var x in Model.GetAnnotations().Where(a => a.StartsWith(batchPrefix)))
	    {    	
	        batchNameComboBox.Items.Add(x.Substring(batchPrefixLen));    
	    }

	    // Populate summary tree view
	    NextStep(2);
	}
};

batchNameComboBox.SelectedValueChanged += (System.Object sender, System.EventArgs e) => {

    batchName = batchNameComboBox.Text;
    batchNameFull = batchPrefix + batchName;
    string ann = Model.GetAnnotation(batchNameFull);
    
    if (ann.Contains("_"))
    {
    	processingType = ann.Substring(0,ann.IndexOf("_"));
    	summarySequenceCheckBox.Checked = true;
    	summaryMaxPNumeric.Visible = true;    	
    	maxPLabel.Visible = true;
    	summaryMaxPNumeric.Value = Convert.ToInt32(ann.Substring(ann.IndexOf("_")+1));
    }
    else
    {
    	processingType = ann;
    	summarySequenceCheckBox.Checked = false;
    	summaryMaxPNumeric.Visible = false;
    	maxPLabel.Visible = false;
    }
    
	summarytypeComboBox.Text = processingType;

	// update summary tree view
    NextStep(2); 
};

scriptButton.Click += (System.Object sender, System.EventArgs e) => {

    // Save export script to desktop
    string desktopPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
    System.IO.File.WriteAllText(desktopPath + @"\"+batchNameFull+".cs", sb_ExportScript.ToString());

    Info("A script to generate Processing Batch '"+batchName+"' has been saved to the desktop.");
};

backButton.Click += (System.Object sender, System.EventArgs e) => {

	summaryTreeView.Visible = false;
	scriptButton.Visible = false;
	batchNameComboBox.Visible = false;
	batchNameTextBox.Visible = true;
	typeComboBox.Visible = true;
	summarytypeComboBox.Visible = false;
	infoLabel.Text = infoList[0];
	treeView.Visible = true;
	backButton.Visible = false;
	deleteButton.Visible = true;
	saveButton.Visible = true;
	summaryButton.Visible = true;
	summarySequenceCheckBox.Visible = false;
	sequenceCheckBox.Visible = true;

	batchName = batchNameTextBox.Text;
	batchNameFull = batchPrefix+batchName;
	processingType = typeComboBox.Text;

	maxPLabel.ForeColor = hiddenColor;
	summaryMaxPNumeric.Visible = false;

	if (seqEnabled)
	{
		sequenceCheckBox.Visible = true;
		maxPNumeric.Visible = true;
		maxPLabel.ForeColor = visibleColor;
		maxPLabel.Visible = true;
	}
};

sequenceCheckBox.CheckStateChanged += (System.Object sender, System.EventArgs e) => {

	if (sequenceCheckBox.Checked == true)
	{
		maxPLabel.Visible = true;
		maxPNumeric.Visible = true;
		seqEnabled = true;
		maxPLabel.ForeColor = visibleColor;
	}
	else
	{
		maxPLabel.Visible = false;
		maxPNumeric.Visible = false;
		seqEnabled = false;
		maxPLabel.ForeColor = hiddenColor;
	}
};

newForm.Show();